using Hidistro.ControlPanel.Config;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.agent
{
    public partial class AddAgentInfo : AdminPage
    {
        private void btnCreate_Click(object sender, System.EventArgs e)
        {
            /*
            //实例会员信息j
            MemberInfo member = new MemberInfo();
            string generateId = Globals.GetGenerateId();
            member.GradeId = new MemberGradeDao().GetDefaultMemberGrade();
            member.UserName = this.txtUserName.Text.Trim();
            member.CreateDate = DateTime.Now;
            member.SessionId = generateId;
            member.SessionEndTime = DateTime.Now.AddYears(10);
            member.Password = HiCryptographer.Md5Encrypt("888888");
            if ((new MemberDao().GetusernameMember(member.UserName) == null) && new MemberDao().CreateMember(member))
            {
                //根据推荐人获取相关信息
                string referralPath = string.Empty;
                string agentPath = string.Empty;
                string distributorname = this.txtReferralUserId.Text;
                int referruserId = MemberHelper.IsExiteDistributorNames(distributorname);
                DistributorGrade threeDistributor = DistributorGrade.ThreeDistributor;
                if (referruserId > 0)
                {
                    referralPath = new DistributorsDao().GetDistributorInfo(referruserId).ReferralPath;
                    agentPath = string.IsNullOrEmpty(referralPath) ? referruserId.ToString() : referralPath + "|" + referruserId.ToString();
                    if (string.IsNullOrEmpty(referralPath))
                    {
                        referralPath = referruserId.ToString();
                        threeDistributor = DistributorGrade.TowDistributor;
                    }
                    else if (referralPath.Contains("|"))
                    {
                        referralPath = referralPath.Split(new char[] { '|' })[1] + "|" + referruserId.ToString();
                    }
                    else
                    {
                        referralPath = referralPath + "|" + referruserId.ToString();
                    }
                }

                //实例分销商信息
                DistributorsInfo distributor = new DistributorsInfo
                {
                    UserId = new MemberDao().GetusernameMember(member.UserName).UserId,
                    RequestAccount = "",
                    StoreName = txtStoreName.Text.Trim(),
                    StoreDescription = "",
                    BackImage = "",
                    Logo = "",
                    DistributorGradeId = threeDistributor,
                    IsAgent = 1,
                    AgentGradeId = int.Parse(ddlAgentGrade.SelectedValue.ToString()),
                    AgentPath = agentPath
                };
                distributor.UserId.ToString();
                distributor.ReferralPath = referralPath;
                distributor.ParentUserId = new int?(Convert.ToInt32(referruserId));
                DistributorGradeInfo isDefaultDistributorGradeInfo = new DistributorsDao().GetIsDefaultDistributorGradeInfo();
                distributor.DistriGradeId = isDefaultDistributorGradeInfo.GradeId;
                if (new DistributorsDao().CreateAgent(distributor))
                {
                    this.ShowMsgAndReUrl("添加成功！", true, "../distributor/distributorlist.aspx?isagent=1");
                }
            }
            else
            {
                this.ShowMsg(string.Format("账号【{0}】已存在", member.UserName), false);
            }
             */

            if (btnCreate.Text == "返 回") {
                Response.Redirect("../member/ManageMembersEx.aspx");
                return;
            }
            //判断自己的店铺名是否重复
            string myStoreName = txtStoreName.Text.Trim();
            DistributorsInfo info=DistributorsBrower.GetDistributorInfo(Request.QueryString["userId"].ToInt());
            if (MemberHelper.IsExiteDistributorNames(myStoreName) > 0 && info==null)//如果同名,则不能继续,否则取出来的id就是同名的代理商id了
            {
                this.ShowMsg("您的店铺名重复！", false);
                return;
            }

            //根据推荐人获取相关信息
            string referralPath = string.Empty;
            string agentPath = string.Empty;
            string distributorname = this.txtReferralUserId.Text;
            int referruserId = MemberHelper.IsExiteDistributorNames(distributorname);

            DistributorGrade threeDistributor = DistributorGrade.ThreeDistributor;
            if (referruserId > 0)
            {
                referralPath = new DistributorsDao().GetDistributorInfo(referruserId).ReferralPath;
                agentPath = string.IsNullOrEmpty(referralPath) ? referruserId.ToString() : referralPath + "|" + referruserId.ToString();
                if (string.IsNullOrEmpty(referralPath))
                {
                    referralPath = referruserId.ToString();
                    threeDistributor = DistributorGrade.TowDistributor;
                }
                else if (referralPath.Contains("|"))
                {
                    referralPath = referralPath.Split(new char[] { '|' })[1] + "|" + referruserId.ToString();
                }
                else
                {
                    referralPath = referralPath + "|" + referruserId.ToString();
                }
            }
            else if (referruserId == 0)//如果没有推荐商,ReferralUserid就是自己
            {
                referruserId = int.Parse(Request.QueryString["userId"]);
            }

            //获取会员信息、分销信息
            MemberInfo member = MemberHelper.GetMember(int.Parse(Request.QueryString["userId"]));
            DistributorsInfo distributor = VShopHelper.GetUserIdDistributors(member.UserId);

            //升级会员为代理
            if (distributor == null)
            {
                DistributorGradeInfo isDefaultDistributorGradeInfo = new DistributorsDao().GetIsDefaultDistributorGradeInfo();
                distributor = new DistributorsInfo
                {
                    UserId = new MemberDao().GetusernameMember(member.UserName).UserId,
                    RequestAccount = "",
                    StoreName = txtStoreName.Text.Trim(),
                    StoreDescription = "",
                    BackImage = "",
                    Logo = "",
                    DistributorGradeId = threeDistributor,
                    IsAgent = 1,
                    DistriGradeId = isDefaultDistributorGradeInfo.GradeId,
                    AgentGradeId = int.Parse(ddlAgentGrade.SelectedValue.ToString()),
                    AgentPath = agentPath,
                    ReferralPath = referralPath,
                    ParentUserId = new int?(Convert.ToInt32(referruserId)),
                    ReferralUserId = referruserId //
                };
                new DistributorsDao().CreateAgent(distributor);
            }
            else
            {
                //升级分销为代理
                string where="UserId="+distributor.UserId;
                DataTable dtData = DistributorsBrower.GetDistributorsByWhere(where);
                string oldReferralPath = (dtData.Rows[0]["ReferralPath"] == DBNull.Value) ? "" : dtData.Rows[0]["ReferralPath"].ToString();
                dtData.Rows[0]["IsAgent"] = 1;
                dtData.Rows[0]["AgentGradeId"] = int.Parse(ddlAgentGrade.SelectedValue.ToString());
                if (!CustomConfigHelper.Instance.SelectServerAgent && referruserId == 0)
                {
                    dtData.Rows[0]["AgentPath"] = agentPath;
                    dtData.Rows[0]["ReferralPath"] = referralPath;
                    dtData.Rows[0]["ReferralUserid"] = referruserId;//referraluserid更新
                    //DistributorsBrower.CommitDistributors(dtData);

                    //找到所有的子
                    dtData.PrimaryKey = new DataColumn[] { dtData.Columns["UserID"] };
                    FindCurrDistributorChild(distributor.UserId, dtData);
                    agentPath = (agentPath == "") ? distributor.UserId.ToString() : (agentPath + "|" + distributor.UserId.ToString());

                    //设置所属的 代理商
                    foreach (DataRow dr in dtData.Rows)
                    {
                        //清除缓存
                        DistributorsBrower.RemoveDistributorCache(dr["userid"].ToInt());

                        //自己已经设置过
                        if (distributor.UserId == dr["UserID"].ToInt()) continue;

                        //设置所属代理
                        dr["AgentPath"] = agentPath;
                    }

                    //设置所属的 店铺
                    where = string.Format("1=1 AND (ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}')"
                        , distributor.UserId);
                    DataRow[] rowsChildDistributor = dtData.Select(where);
                    foreach (DataRow dr in dtData.Rows)
                    {
                        if (oldReferralPath != "")
                        {
                            string currReferralPath = dr["ReferralPath"].ToString();
                            if (currReferralPath.IndexOf(oldReferralPath) == 0)
                            {
                                if (referralPath == "")
                                    currReferralPath = distributor.UserId.ToString();
                                else
                                {
                                    //替换前缀
                                    currReferralPath = referralPath + "|" + currReferralPath.Split('|')[currReferralPath.Split('|').Length - 1];
                                }
                                dr["ReferralPath"] = currReferralPath;
                            }
                        }

                    }
                }
                //保存到数据库
                DistributorsBrower.CommitDistributors(dtData);
            }
            this.ShowMsgAndReUrl("升级成功！", true, "../member/ManageMembersEx.aspx");
        }

         /// <summary>
        /// 查找当前店铺的所有子店铺
        /// </summary>
        private void FindCurrDistributorChild(int currUserID, DataTable dtData)
        {
            string where = string.Format("1=1 AND (ReferralPath LIKE '{0}|%' OR ReferralPath LIKE '%|{0}|%' OR ReferralPath LIKE '%|{0}' OR ReferralPath='{0}')"
                    , currUserID);
            DataTable dtChild = DistributorsBrower.GetDistributorsByWhere(where);
            foreach (DataRow dr in dtChild.Rows)
            {
                if (dtData.Rows.Find(dr["UserID"]) != null) continue;
                dtData.Rows.Add(dr.ItemArray);
                dtData.Rows[dtData.Rows.Count - 1].AcceptChanges();
                FindCurrDistributorChild(dr["UserID"].ToInt(), dtData);
            }
        }

      


        /*
        protected void confirm()
        {

            string cfm = string.Format(@"确认一下信息无误吗?\n代理商账号:{0}\n代理商等级:{1}\n店铺名称:{2}\n推荐代理商{3}",txtUserName.Text,ddlAgentGrade,txtStoreName,txtReferralUserId);
            Response.Write(@"<script>
        if ( window.confirm('"+cfm+"')) { window.location.href='" + strUrl_Yes + "' } else {window.location.href='"+ strUrl_No +"' };
</script>");
        }
        */

        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(base.Request["action"]) && base.Request["action"] == "SearchKey")
            {
                string allDistributorsName = string.Empty;
                if (!string.IsNullOrEmpty(base.Request["keyword"]))
                {
                    allDistributorsName = MemberHelper.GetAgentDistributorsName(base.Request["keyword"]);
                }
                base.Response.ContentType = "application/json";
                base.Response.Write("{\"data\":[" + allDistributorsName + "]}");
                base.Response.End();
            }
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            if (!this.Page.IsPostBack)
            {
                string userId = Request.QueryString["userId"];
                if (String.IsNullOrEmpty(userId))
                {
                    Response.Write("手动添加功能已关闭，请到会员管理中进行升级为代理商！");
                    Response.End();
                }

                MemberInfo member = MemberHelper.GetMember(int.Parse(userId));
                DistributorsInfo userIdDistributors = VShopHelper.GetUserIdDistributors(int.Parse(userId));
                if(member==null || String.IsNullOrEmpty(member.UserName)){
                    Response.Write("手动添加功能已关闭，请到会员管理中进行升级为代理商！");
                    Response.End();
                }

                this.litTitle.Text = string.Format("当前操作的用户【{0}】为会员，{1}申请为分销商", member.UserName, (userIdDistributors == null) ? "未" : "已");
                this.txtUserName.Text = member.UserName;
                if (userIdDistributors != null) {
                    this.txtStoreName.Text = userIdDistributors.StoreName;
                    this.txtStoreName.Enabled = false;
                    if (userIdDistributors.IsAgent == 1)
                    {
                        btnCreate.Text = "返 回";
                        this.ShowMsg(string.Format("用户【{0}】已经理是代理", member.UserName), false);
                    }
                }
                
                this.ddlAgentGrade.DataBind();
                this.ddlAgentGrade.Items.Insert(0,new ListItem("--请选择--",""));
            }
        }
    }
}
