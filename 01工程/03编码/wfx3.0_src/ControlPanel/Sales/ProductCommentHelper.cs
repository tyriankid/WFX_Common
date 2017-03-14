﻿namespace Hidistro.ControlPanel.Sales
{
    using Hidistro.Core.Entities;
    using Hidistro.Entities.Comments;
    using Hidistro.Entities.Store;
    using Hidistro.SqlDal.Comments;
    using System;
    using System.Collections.Generic;

    public sealed class ProductCommentHelper
    {
        private ProductCommentHelper()
        {
        }

        public static int DeleteFriendExtension(int id)
        {
            return new FriendExtensionDao().DeleteFriendExtension(id);
        }

        public static int DeleteProductConsultation(int consultationId)
        {
            return new ProductConsultationDao().DeleteProductConsultation(consultationId);
        }

        public static int DeleteProductReview(long reviewId)
        {
            return new ProductReviewDao().DeleteProductReview(reviewId);
        }

        public static int DeleteReview(IList<long> reviews)
        {
            if ((reviews == null) || (reviews.Count == 0))
            {
                return 0;
            }
            int num = 0;
            foreach (long num2 in reviews)
            {
                new ProductReviewDao().DeleteProductReview(num2);
                num++;
            }
            return num;
        }

        public static DbQueryResult FriendExtensionList(FriendExtensionQuery Query)
        {
            return new FriendExtensionDao().FriendExtensionList(Query);
        }

        public static DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
        {
            return new ProductConsultationDao().GetConsultationProducts(consultationQuery);
        }

        public static ProductConsultationInfo GetProductConsultation(int consultationId)
        {
            return new ProductConsultationDao().GetProductConsultation(consultationId);
        }

        public static ProductReviewInfo GetProductReview(int reviewId)
        {
            return new ProductReviewDao().GetProductReview(reviewId);
        }

        public static DbQueryResult GetProductReviews(ProductReviewQuery reviewQuery)
        {
            return new ProductReviewDao().GetProductReviews(reviewQuery);
        }

        public static bool InsertFriendExtension(FriendExtensionInfo Query)
        {
            return new FriendExtensionDao().InsertFriendExtension(Query);
        }

        public static bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
        {
            return new ProductConsultationDao().ReplyProductConsultation(productConsultation);
        }

        public static bool UpdateFriendExtension(FriendExtensionInfo Query)
        {
            return new FriendExtensionDao().UpdateFriendExtension(Query);
        }

        public static bool UpdateFriendExtensionImg(FriendExtensionInfo Query)
        {
            return new FriendExtensionDao().UpdateFriendExtensionImg(Query);
        }
    }
}

