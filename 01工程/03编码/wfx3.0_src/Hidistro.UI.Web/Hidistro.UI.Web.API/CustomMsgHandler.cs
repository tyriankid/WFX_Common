using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hishop.Weixin.MP;
using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Handler;
using Hishop.Weixin.MP.Request;
using Hishop.Weixin.MP.Request.Event;
using Hishop.Weixin.MP.Response;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;

namespace Hidistro.UI.Web.API
{

    public class CustomMsgHandler : RequestHandler
    {

        public CustomMsgHandler(System.IO.Stream inputStream) : base(inputStream) { }

        public CustomMsgHandler(string xml) : base(xml) { }


        public override AbstractResponse DefaultResponse(AbstractRequest requestMessage)
        {
            ReplyInfo mismatchReply = ReplyHelper.GetMismatchReply();
            AbstractResponse result;
            if (mismatchReply == null || this.IsOpenManyService())
            {
                result = this.GotoManyCustomerService(requestMessage);
            }
            else
            {
                AbstractResponse response = this.GetResponse(mismatchReply, requestMessage.FromUserName);
                if (response == null)
                {
                    result = this.GotoManyCustomerService(requestMessage);
                }
                else
                {
                    response.ToUserName = requestMessage.FromUserName;
                    response.FromUserName = requestMessage.ToUserName;
                    result = response;
                }
            }
            return result;
        }


        private AbstractResponse GetKeyResponse(string key, AbstractRequest request)
        {
            System.Collections.Generic.IList<ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Topic);
            AbstractResponse result;
            if (replies != null && replies.Count > 0)
            {
                foreach (ReplyInfo info in replies)
                {
                    if (info.Keys == key)
                    {
                        TopicInfo topic = VShopHelper.Gettopic(info.ActivityId);
                        if (topic != null)
                        {
                            NewsResponse response = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article item = new Article
                            {
                                Description = topic.Title,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, topic.IconUrl),
                                Title = topic.Title,
                                Url = string.Format("http://{0}/vshop/Topics.aspx?TopicId={1}", System.Web.HttpContext.Current.Request.Url.Host, topic.TopicId)
                            };
                            response.Articles.Add(item);
                            result = response;
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> list2 = ReplyHelper.GetReplies(ReplyType.Vote);
            if (list2 != null && list2.Count > 0)
            {
                foreach (ReplyInfo info2 in list2)
                {
                    if (info2.Keys == key)
                    {
                        VoteInfo voteById = StoreHelper.GetVoteById((long)info2.ActivityId);
                        if (voteById != null && voteById.IsBackup)
                        {
                            NewsResponse response2 = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article article2 = new Article
                            {
                                Description = voteById.VoteName,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, voteById.ImageUrl),
                                Title = voteById.VoteName,
                                Url = string.Format("http://{0}/vshop/Vote.aspx?voteId={1}", System.Web.HttpContext.Current.Request.Url.Host, voteById.VoteId)
                            };
                            response2.Articles.Add(article2);
                            result = response2;
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> list3 = ReplyHelper.GetReplies(ReplyType.Wheel);
            if (list3 != null && list3.Count > 0)
            {
                foreach (ReplyInfo info3 in list3)
                {
                    if (info3.Keys == key)
                    {
                        LotteryActivityInfo lotteryActivityInfo = VShopHelper.GetLotteryActivityInfo(info3.ActivityId);
                        if (lotteryActivityInfo != null)
                        {
                            NewsResponse response3 = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article article3 = new Article
                            {
                                Description = lotteryActivityInfo.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryActivityInfo.ActivityPic),
                                Title = lotteryActivityInfo.ActivityName,
                                Url = string.Format("http://{0}/vshop/BigWheel.aspx?activityId={1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryActivityInfo.ActivityId)
                            };
                            response3.Articles.Add(article3);
                            result = response3;
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> list4 = ReplyHelper.GetReplies(ReplyType.Scratch);
            if (list4 != null && list4.Count > 0)
            {
                foreach (ReplyInfo info4 in list4)
                {
                    if (info4.Keys == key)
                    {
                        LotteryActivityInfo info5 = VShopHelper.GetLotteryActivityInfo(info4.ActivityId);
                        if (info5 != null)
                        {
                            NewsResponse response4 = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article article4 = new Article
                            {
                                Description = info5.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, info5.ActivityPic),
                                Title = info5.ActivityName,
                                Url = string.Format("http://{0}/vshop/Scratch.aspx?activityId={1}", System.Web.HttpContext.Current.Request.Url.Host, info5.ActivityId)
                            };
                            response4.Articles.Add(article4);
                            result = response4;
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> list5 = ReplyHelper.GetReplies(ReplyType.SmashEgg);
            if (list5 != null && list5.Count > 0)
            {
                foreach (ReplyInfo info6 in list5)
                {
                    if (info6.Keys == key)
                    {
                        LotteryActivityInfo info7 = VShopHelper.GetLotteryActivityInfo(info6.ActivityId);
                        if (info7 != null)
                        {
                            NewsResponse response5 = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article article5 = new Article
                            {
                                Description = info7.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, info7.ActivityPic),
                                Title = info7.ActivityName,
                                Url = string.Format("http://{0}/vshop/SmashEgg.aspx?activityId={1}", System.Web.HttpContext.Current.Request.Url.Host, info7.ActivityId)
                            };
                            response5.Articles.Add(article5);
                            result = response5;
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> list6 = ReplyHelper.GetReplies(ReplyType.SignUp);
            if (list6 != null && list6.Count > 0)
            {
                foreach (ReplyInfo info8 in list6)
                {
                    if (info8.Keys == key)
                    {
                        ActivityInfo activity = VShopHelper.GetActivity(info8.ActivityId);
                        if (activity != null)
                        {
                            NewsResponse response6 = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article article6 = new Article
                            {
                                Description = activity.Description,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, activity.PicUrl),
                                Title = activity.Name,
                                Url = string.Format("http://{0}/vshop/Activity.aspx?id={1}", System.Web.HttpContext.Current.Request.Url.Host, activity.ActivityId)
                            };
                            response6.Articles.Add(article6);
                            result = response6;
                            return result;
                        }
                    }
                }
            }
            System.Collections.Generic.IList<ReplyInfo> list7 = ReplyHelper.GetReplies(ReplyType.Ticket);
            if (list7 != null && list7.Count > 0)
            {
                foreach (ReplyInfo info9 in list7)
                {
                    if (info9.Keys == key)
                    {
                        LotteryTicketInfo lotteryTicket = VShopHelper.GetLotteryTicket(info9.ActivityId);
                        if (lotteryTicket != null)
                        {
                            NewsResponse response7 = new NewsResponse
                            {
                                CreateTime = System.DateTime.Now,
                                FromUserName = request.ToUserName,
                                ToUserName = request.FromUserName,
                                Articles = new System.Collections.Generic.List<Article>()
                            };
                            Article article7 = new Article
                            {
                                Description = lotteryTicket.ActivityDesc,
                                PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryTicket.ActivityPic),
                                Title = lotteryTicket.ActivityName,
                                Url = string.Format("http://{0}/vshop/SignUp.aspx?id={1}", System.Web.HttpContext.Current.Request.Url.Host, lotteryTicket.ActivityId)
                            };
                            response7.Articles.Add(article7);
                            result = response7;
                            return result;
                        }
                    }
                }
            }
            result = null;
            return result;
        }

        public AbstractResponse GetResponse(ReplyInfo reply, string openId)
        {
            AbstractResponse result;
            if (reply.MessageType == MessageType.Text)
            {
                TextReplyInfo info = reply as TextReplyInfo;
                TextResponse response = new TextResponse
                {
                    CreateTime = System.DateTime.Now,
                    Content = info.Text
                };
                if (reply.Keys == "登录")
                {
                    string str = string.Format("http://{0}/Vshop/Login.aspx?SessionId={1}", System.Web.HttpContext.Current.Request.Url.Host, openId);
                    response.Content = response.Content.Replace("$login$", string.Format("<a href=\"{0}\">一键登录</a>", str));
                }
                result = response;
            }
            else
            {
                NewsResponse response2 = new NewsResponse
                {
                    CreateTime = System.DateTime.Now,
                    Articles = new System.Collections.Generic.List<Article>()
                };
                foreach (NewsMsgInfo info2 in (reply as NewsReplyInfo).NewsMsg)
                {
                    Article item = new Article
                    {
                        Description = info2.Description,
                        PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, info2.PicUrl),
                        Title = info2.Title,
                        Url = string.IsNullOrEmpty(info2.Url) ? string.Format("http://{0}/Vshop/ImageTextDetails.aspx?messageId={1}", System.Web.HttpContext.Current.Request.Url.Host, info2.Id) : info2.Url
                    };
                    response2.Articles.Add(item);
                }
                result = response2;
            }
            return result;
        }

        public AbstractResponse GotoManyCustomerService(AbstractRequest requestMessage)
        {
            AbstractResponse result;
            if (!this.IsOpenManyService())
            {
                result = null;
            }
            else
            {
                result = new AbstractResponse
                {
                    FromUserName = requestMessage.ToUserName,
                    ToUserName = requestMessage.FromUserName,
                    MsgType = ResponseMsgType.transfer_customer_service
                };
            }
            return result;
        }

        public bool IsOpenManyService()
        {
            return SettingsManager.GetMasterSettings(false).OpenManyService;
        }

        public override AbstractResponse OnEvent_ClickRequest(ClickEventRequest clickEventRequest)
        {
            MenuInfo menu = VShopHelper.GetMenu(System.Convert.ToInt32(clickEventRequest.EventKey));
            AbstractResponse result;
            if (menu == null)
            {
                result = null;
            }
            else
            {
                ReplyInfo reply = ReplyHelper.GetReply(menu.ReplyId);
                if (reply == null)
                {
                    result = null;
                }
                else
                {
                    AbstractResponse keyResponse = this.GetKeyResponse(reply.Keys, clickEventRequest);
                    if (keyResponse != null)
                    {
                        result = keyResponse;
                    }
                    else
                    {
                        AbstractResponse response = this.GetResponse(reply, clickEventRequest.FromUserName);
                        if (response == null)
                        {
                            this.GotoManyCustomerService(clickEventRequest);
                        }
                        response.ToUserName = clickEventRequest.FromUserName;
                        response.FromUserName = clickEventRequest.ToUserName;
                        result = response;
                    }
                }
            }
            return result;
        }

        //取消关注事件
        public override AbstractResponse OnEvent_UnSubscribeRequest(UnSubscribeEventRequest unsubscribeEventRequest)
        {
            //string[] arrayEventKey = unsubscribeEventRequest.EventKey.Split('_');

            //爽爽挝啡需求,取消关注减少粉丝数(网aspnet_storefans表内插入一个subscribetype为1的数据,取消关注)
            try
            {
                //获取取消关注用户的openid
                string openid=unsubscribeEventRequest.FromUserName;
                MemberInfo unSubMember = MemberProcessor.GetOpenIdMember(openid);
                //插入取消关注数据
                if (unSubMember != null)
                    ManagerHelper.addStoreFansCount(-1, unSubMember.UserId, 1);
                //ManagerHelper.addStoreFansCount(storeId.ToInt(), member.UserId,1);
            }
            catch (Exception ex)
            {
                WriteLog("取消关注异常:" + ex.Message);
            }
            

            return null;
        }

        public override AbstractResponse OnEvent_SubscribeRequest(SubscribeEventRequest subscribeEventRequest)
        {
            WriteLog("GetSubscribeReply");
            ReplyInfo subscribeReply = ReplyHelper.GetSubscribeReply();
            AbstractResponse result;
            
            if (subscribeReply == null)
            {
                WriteLog("subscribeReply == null");
                result = null;
            }
            else
            {
                WriteLog("取消关注再关注,进入这个事件");
                subscribeReply.Keys = "登录";
                AbstractResponse response = this.GetResponse(subscribeReply, subscribeEventRequest.FromUserName);
                
                if (response == null)
                {
                    this.GotoManyCustomerService(subscribeEventRequest);
                }
                response.ToUserName = subscribeEventRequest.FromUserName;
                response.FromUserName = subscribeEventRequest.ToUserName;
                result = response;
                //取消关注再关注,进入这个事件
                ScanVisitDistributor(subscribeEventRequest, subscribeEventRequest.EventKey);
                
            }
            return result;
        }
        public override AbstractResponse OnTextRequest(TextRequest textRequest)
        {
            AbstractResponse keyResponse = this.GetKeyResponse(textRequest.Content, textRequest);
            AbstractResponse result;
            if (keyResponse != null)
            {
                result = keyResponse;
            }
            else
            {
                System.Collections.Generic.IList<ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Keys);
                if (replies == null || (replies.Count == 0 && this.IsOpenManyService()))
                {
                    this.GotoManyCustomerService(textRequest);
                }
                foreach (ReplyInfo info in replies)
                {
                    if (info.MatchType == MatchType.Equal && info.Keys == textRequest.Content)
                    {
                        AbstractResponse response = this.GetResponse(info, textRequest.FromUserName);
                        response.ToUserName = textRequest.FromUserName;
                        response.FromUserName = textRequest.ToUserName;
                        result = response;
                        return result;
                    }
                    if (info.MatchType == MatchType.Like && info.Keys.Contains(textRequest.Content))
                    {
                        AbstractResponse response2 = this.GetResponse(info, textRequest.FromUserName);
                        response2.ToUserName = textRequest.FromUserName;
                        response2.FromUserName = textRequest.ToUserName;
                        result = response2;
                        return result;
                    }
                }
                result = this.DefaultResponse(textRequest);
            }
            return result;
        }

        /// <summary>
        /// 增加扫码事件处理,add by jhb,20151213
        /// </summary>
        /// <param name="scanEventRequest"></param>
        /// <returns></returns>
        public override AbstractResponse OnEvent_ScanRequest(ScanEventRequest scanEventRequest)
        {
            ScanVisitDistributor(scanEventRequest, scanEventRequest.EventKey);
            return null;
        }
        public override AbstractResponse OnEvent_ViewRequest(ViewEventRequest viewEventRequest)
        {
            ScanVisitDistributor(viewEventRequest, viewEventRequest.EventKey);
            return null;
        }


        /// <summary>
        /// 关注访问店铺处理
        /// </summary>
        private void ScanVisitDistributor(EventRequest eventRequest, string strEventKey)
        {
            WriteLog("1.响应微信消息事件:" + eventRequest.Event);
            if (!string.IsNullOrEmpty(strEventKey) && strEventKey.Split('_').Length>1)
            {
                WriteLog("2.响应微信消息内容:" + strEventKey);
                string[] arrayEventKey = strEventKey.Split('_');
                switch (arrayEventKey[arrayEventKey.Length-2].ToLower())
                {
                    case "distributor":
                        WriteLog("3.进入枚举_访问店铺处理:" + arrayEventKey[arrayEventKey.Length - 1]);
                        string VisitDistributorID = HiCache.Get(string.Format("DataCache-VisitDistributor-{0}", arrayEventKey[arrayEventKey.Length - 1])) as string;
                        if (string.IsNullOrEmpty(VisitDistributorID))
                        {
                            WriteLog("4.存储访问店铺ID:" + VisitDistributorID);
                            HiCache.Remove(string.Format("DataCache-VisitDistributor-{0}", eventRequest.FromUserName));
                            HiCache.Insert(string.Format("DataCache-VisitDistributor-{0}", eventRequest.FromUserName), arrayEventKey[arrayEventKey.Length - 1], 360
                                , System.Web.Caching.CacheItemPriority.Normal);
                            WriteLog("5.成功存储访问店铺ID:" );
                        }
                        break;
                    case "frommember":
                        WriteLog("进入frommember");
                        //设置推荐人id
                        string VisitFromMemberId = HiCache.Get(string.Format("DataCache-FromMemberId-{0}", arrayEventKey[arrayEventKey.Length - 1])) as string;
                        if (string.IsNullOrEmpty(VisitFromMemberId))
                        {
                            HiCache.Remove(string.Format("DataCache-FromMemberId-{0}", eventRequest.FromUserName));
                            HiCache.Insert(string.Format("DataCache-FromMemberId-{0}", eventRequest.FromUserName), arrayEventKey[arrayEventKey.Length - 1], 360
                                , System.Web.Caching.CacheItemPriority.Normal);
                            WriteLog("推荐人openid:"+eventRequest.FromUserName+" " +"推荐人id:"+arrayEventKey[arrayEventKey.Length - 1]);
                        }
                        break;
                    case "storesenderid":
                        WriteLog("进入StoreSenderId");

                        string VisitStoreId = HiCache.Get(string.Format("DataCache-sub-StoreId-{0}", arrayEventKey[arrayEventKey.Length - 1])) as string;
                        if (string.IsNullOrEmpty(VisitStoreId))
                        {
                            HiCache.Remove(string.Format("DataCache-sub-StoreId-{0}", eventRequest.FromUserName));
                            HiCache.Insert(string.Format("DataCache-sub-StoreId-{0}", eventRequest.FromUserName), arrayEventKey[arrayEventKey.Length - 1], 360
                                , System.Web.Caching.CacheItemPriority.Normal);
                        }

                        break;
                }
                WriteLog("9.响应微信消息结束:" );
            }
        }

        private void WriteLog(string log)
        {
            System.IO.StreamWriter writer = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/error.txt"));
            writer.WriteLine(System.DateTime.Now);
            writer.WriteLine(log);
            writer.Flush();
            writer.Close();
        }

    }
}
