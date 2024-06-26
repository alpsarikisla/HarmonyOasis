﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccessLayer;

namespace HarmonyOasis.AdminPanel
{
    public partial class MuzisyenDuzenle : System.Web.UI.Page
    {
        DataModel dm = new DataModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count != 0)
            {
                if (!IsPostBack)
                {
                    int id = Convert.ToInt32(Request.QueryString["muzisyenID"]);
                    Muzisyen muz = dm.MuzisyenGetir(id);

                    tb_isim.Text = muz.Isim;
                    tb_ozet.Text = muz.Ozet;
                    tb_content.Text = muz.Biyografi;
                    cb_yayin.Checked = muz.Durum;
                    resim_muz.ImageUrl = "../Resimler/MuzisyenResimleri/" + muz.Resim;
                }
            }
            else
            {
                Response.Redirect("MuzisyenListele.aspx");
            }
        }

        protected void btn_muzisyenDuzenle_Click(object sender, EventArgs e)
        {
            bool ekle = true;
            Muzisyen muz = new Muzisyen();
            muz.ID = Convert.ToInt32(Request.QueryString["muzisyenID"]);
            muz.Isim = tb_isim.Text;
            muz.Ozet = tb_ozet.Text;
            muz.Biyografi = tb_content.Text;
            muz.Durum = cb_yayin.Checked;
            if (fu_resim.HasFile)
            {
                FileInfo fi = new FileInfo(fu_resim.FileName);
                string uzantı = fi.Extension;
                if (uzantı == ".jpg" || uzantı == ".png")
                {
                    string isim = Guid.NewGuid().ToString();
                    muz.Resim = isim + uzantı;
                    fu_resim.SaveAs(Server.MapPath("../Resimler/MuzisyenResimleri/") + isim + uzantı);
                }
                else
                {
                    ekle = false;
                    successful_pnl.Visible = false;
                    lbl_fail.Text = "Resim Uzantısı 'jpg' veya 'png' Olmalıdır";
                    failed_pnl.Visible = true;
                }
            }
            else
            {
                muz.Resim = "none.jpg";
            }

            if (ekle)
            {
                if (!string.IsNullOrEmpty(muz.Isim))
                {
                    if (string.IsNullOrEmpty(muz.Ozet) && string.IsNullOrEmpty(muz.Biyografi) || (!string.IsNullOrEmpty(muz.Ozet) && !string.IsNullOrEmpty(muz.Biyografi)))
                    {
                        if (dm.MuzisyenDuzenle(muz))
                        {
                            failed_pnl.Visible = false;
                            lbl_success.Text = "Müzisyen Ekleme Başarılı";
                            successful_pnl.Visible = true;
                        }
                        else
                        {
                            successful_pnl.Visible = false;
                            lbl_fail.Text = "Müzisyen Eklenirken Bir Hata Oluştu";
                            failed_pnl.Visible = true;
                        }
                    }
                    else
                    {
                        successful_pnl.Visible = false;
                        lbl_fail.Text = "Özet Ve Biyografi Biri Boş Biri Dolu Olamaz";
                        failed_pnl.Visible = true;
                    }
                }
                else
                {
                    successful_pnl.Visible = false;
                    lbl_fail.Text = "Müzisyen İsmi Boş Bırakılamaz";
                    failed_pnl.Visible = true;
                }
            }
        }
    }
}