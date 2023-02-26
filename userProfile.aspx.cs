using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Library_Management_System
{
    public partial class userProfile : System.Web.UI.Page
    {
        string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString; 
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["username"] == null || Session["username"].ToString()== "")
                {
                    Response.Write("<script>alert('Session expired Login again')</script>");
                    Response.Redirect("userLogin.aspx");
                }
                else
                {
                    getUsersBookDetails();
                    if(!Page.IsPostBack)
                    {
                        getUserPersonalDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        // update button
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["username"] == null || Session["username"].ToString() == "")
                {
                    Response.Write("<script>alert('Session expired Login again')</script>");
                    Response.Redirect("userLogin.aspx");
                }
                else
                {
                    updateUserPersonalDetails();
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        // user defined function

        void updateUserPersonalDetails()
        {
            string password = "";
            if (TextBox10.Text.Trim() == "")
            {
                password = TextBox9.Text.Trim();
            }
            else
            {
                password = TextBox10.Text.Trim();
            }
            try
            {
                SqlConnection con=new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("UPDATE member_master_tbl SET full_name=@full_name,dob=@dob,contact_no=@contact_no,email=@email,state=@state,city=@city,pincode=@pincode,full_address=@full_address,password=@password,account_status=@account_status WHERE member_id='" + Session["username"].ToString() +"';", con);
                cmd.Parameters.AddWithValue("@full_name", TextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@dob", TextBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@contact_no", TextBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@email", TextBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@state", DropDownList1.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@city", TextBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@pincode", TextBox7.Text.Trim());
                cmd.Parameters.AddWithValue("@full_address", TextBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@password",password);
                cmd.Parameters.AddWithValue("@account_status", "pending");
                int result=cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    Response.Write("<script>alert('Your Details are updated successfully');<script>");
                    getUserPersonalDetails();
                    getUsersBookDetails();
                }
                else
                {
                    Response.Write("<script>alert('Invalid Entry');<script>");
                }
                
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        void getUserPersonalDetails()
        {
            try
            {
                SqlConnection con=new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM member_master_tbl WHERE member_id='" + Session["username"].ToString() +"';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                TextBox1.Text = dt.Rows[0][0].ToString().Trim();
                TextBox2.Text = dt.Rows[0][1].ToString().Trim();
                TextBox3.Text = dt.Rows[0][2].ToString().Trim();
                TextBox4.Text = dt.Rows[0][3].ToString().Trim();
                DropDownList1.SelectedValue = dt.Rows[0][4].ToString().Trim();
                TextBox6.Text = dt.Rows[0][5].ToString().Trim();
                TextBox7.Text = dt.Rows[0][6].ToString().Trim();
                TextBox5.Text = dt.Rows[0][7].ToString().Trim();
                TextBox8.Text = dt.Rows[0][8].ToString().Trim();
                TextBox9.Text = dt.Rows[0][9].ToString().Trim();
                Label1.Text = dt.Rows[0][10].ToString().Trim();
                if (dt.Rows[0][10].ToString().Trim() == "active")
                {
                    Label1.Attributes.Add("class", "badge rounded-pill bg-success text-white");
                }
                else if (dt.Rows[0][10].ToString().Trim()=="pending")
                {
                    Label1.Attributes.Add("class", "badge rounded-pill bg-warning text-white");
                }
                else if (dt.Rows[0][10].ToString().Trim()== "deactive")
                {
                    Label1.Attributes.Add("class", "badge rounded-pill bg-danger text-white");
                }
                else
                {
                    Label1.Attributes.Add("class", "badge rounded-pill bg-info text-dark");
                }


            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
        void getUsersBookDetails()
        {
            try
            {
                SqlConnection con=new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand("SELECT * FROM book_issue_table WHERE member_id='" + Session["username"].ToString() +"';", con);
                SqlDataAdapter da=new SqlDataAdapter(cmd);
                DataTable dt=new DataTable();
                da.Fill(dt);
                GridView1.DataSource= dt;
                GridView1.DataBind();


            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Check your condition here
                    DateTime dt = Convert.ToDateTime(e.Row.Cells[5].Text);
                    DateTime today = DateTime.Now;
                    if (today > dt)
                    {
                        e.Row.BackColor = System.Drawing.Color.PaleVioletRed;
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }

        
    }
}