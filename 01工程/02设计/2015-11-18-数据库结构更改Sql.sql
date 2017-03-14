/***添加字段ClientUserId***/
ALTER TABLE dbo.aspnet_Managers ADD ClientUserId int NULL

/***添加字段ClientUserId***/
ALTER TABLE dbo.aspnet_Managers ADD AgentName nvarchar(50) NULL

/***创建表 Object:  Table [dbo].[Erp_AgentProduct]    Script Date: 11/18/2015 11:01:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Erp_AgentProduct](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SkuId] [nvarchar](100) NULL,
	[UserId] [int] NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_Erp_AgentProduct] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/***创建表 Object:  Table [dbo].[Erp_ManagersRegion]    Script Date: 11/18/2015 11:03:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Erp_ManagersRegion](
	[MRegionID] [uniqueidentifier] NOT NULL,
	[UserId] [int] NOT NULL,
	[RegionID] [int] NULL,
	[RegionName] [nvarchar](500) NULL,
 CONSTRAINT [PK_Erp_ManagersRegion] PRIMARY KEY CLUSTERED 
(
	[MRegionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/***创建表 Object:  Table [dbo].[Erp_ProductRegion]    Script Date: 11/18/2015 11:04:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Erp_ProductRegion](
	[ID] [uniqueidentifier] NOT NULL,
	[ProductID] [int] NULL,
	[RegionID] [int] NULL,
	[RegionName] [nvarchar](500) NULL,
 CONSTRAINT [PK_Erp_ProductRegion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品区域关系表主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Erp_ProductRegion', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'商品ID-来源Hishop_Products-ProductId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Erp_ProductRegion', @level2type=N'COLUMN',@level2name=N'ProductID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区域ID-来源Erp_Regions-组合ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Erp_ProductRegion', @level2type=N'COLUMN',@level2name=N'RegionID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'区域名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Erp_ProductRegion', @level2type=N'COLUMN',@level2name=N'RegionName'
GO


/***创建表 Object:  Table [dbo].[Erp_Regions]    Script Date: 11/18/2015 11:13:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Erp_Regions](
	[RegionID] [int] NULL,
	[ProvinceID] [int] NULL,
	[CityID] [int] NULL,
	[CountyID] [int] NULL,
	[RegionName] [nvarchar](50) NULL
) ON [PRIMARY]

GO


/***新建表 Object:  Table [dbo].[YiHui_City]    Script Date: 11/18/2015 11:29:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[YiHui_City](
	[CityID] [int] IDENTITY(1,1) NOT NULL,
	[CityName] [nvarchar](50) NOT NULL,
	[ProID] [int] NULL,
	[CitySort] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

/***新建表 Object:  Table [dbo].[YiHui_MenuInfo]    Script Date: 11/18/2015 11:29:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[YiHui_MenuInfo](
	[MIID] [uniqueidentifier] NOT NULL,
	[MIName] [varchar](20) NULL,
	[Layout] [varchar](6) NULL,
	[MIUrl] [varchar](100) NULL,
	[Visible] [int] NOT NULL,
	[Memo] [varchar](500) NULL,
	[IconLink] [varchar](100) NULL,
 CONSTRAINT [PK_YiHui_MenuInfo] PRIMARY KEY CLUSTERED 
(
	[MIID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'后台菜单唯一标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'YiHui_MenuInfo', @level2type=N'COLUMN',@level2name=N'MIID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'YiHui_MenuInfo', @level2type=N'COLUMN',@level2name=N'MIName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单层级' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'YiHui_MenuInfo', @level2type=N'COLUMN',@level2name=N'Layout'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单的url' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'YiHui_MenuInfo', @level2type=N'COLUMN',@level2name=N'MIUrl'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否显示,0不显示,1显示' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'YiHui_MenuInfo', @level2type=N'COLUMN',@level2name=N'Visible'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'YiHui_MenuInfo', @level2type=N'COLUMN',@level2name=N'Memo'
GO

ALTER TABLE [dbo].[YiHui_MenuInfo] ADD  CONSTRAINT [DF_YiHui_MenuInfo_MIID]  DEFAULT (newid()) FOR [MIID]
GO

ALTER TABLE [dbo].[YiHui_MenuInfo] ADD  CONSTRAINT [DF_YiHui_MenuInfo_Visible]  DEFAULT ((1)) FOR [Visible]
GO


/****** Object:  Table [dbo].[YiHui_MenuRelation]    Script Date: 11/18/2015 11:31:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[YiHui_MenuRelation](
	[MRId] [uniqueidentifier] NOT NULL,
	[MRRoleId] [int] NULL,
	[MRMenuId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_YiHui_MenuRelation] PRIMARY KEY CLUSTERED 
(
	[MRId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'后台菜单关系唯一标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'YiHui_MenuRelation', @level2type=N'COLUMN',@level2name=N'MRId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'YiHui_MenuRelation', @level2type=N'COLUMN',@level2name=N'MRRoleId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'YiHui_MenuRelation', @level2type=N'COLUMN',@level2name=N'MRMenuId'
GO

ALTER TABLE [dbo].[YiHui_MenuRelation] ADD  CONSTRAINT [DF_YiHui_MenuRelation_MRId]  DEFAULT (newid()) FOR [MRId]
GO



GO
/****** Object:  View [dbo].[vw_Hishop_BrowseProductSKUList]    Script Date: 11/18/2015 14:27:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_BrowseProductSKUList]
AS
SELECT     p.CategoryId, p.TypeId, p.BrandId, p.ProductId, p.ProductName, p.ProductCode, p.ShortDescription, p.MarketPrice, p.ThumbnailUrl40, p.ThumbnailUrl60, 
                      p.ThumbnailUrl100, p.ThumbnailUrl160, p.ThumbnailUrl180, p.ThumbnailUrl220, p.ThumbnailUrl310, p.SaleStatus, p.DisplaySequence, p.MainCategoryPath, 
                      p.ExtendCategoryPath, p.SaleCounts, p.ShowSaleCounts, p.AddedDate, p.VistiCounts,
                          (SELECT     COUNT(*) AS Expr1
                            FROM          dbo.Taobao_Products
                            WHERE      (ProductId = p.ProductId)) AS IsMakeTaobao, SKU.SkuId, SKU.SKU, SKU.Weight, SKU.Stock, SKU.CostPrice, SKU.SalePrice
FROM         dbo.Hishop_Products AS p INNER JOIN
                      dbo.Hishop_SKUs AS SKU ON p.ProductId = SKU.ProductId
GO



GO
/****** Object:  View [dbo].[vw_Hishop_BrowseProductAgentList]    Script Date: 11/18/2015 14:27:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_Hishop_BrowseProductAgentList]
AS
SELECT     p.CategoryId, p.TypeId, p.BrandId, p.ProductId, p.ProductName, p.ProductCode, p.ShortDescription, p.MarketPrice, p.ThumbnailUrl40, p.ThumbnailUrl60, 
                      p.ThumbnailUrl100, p.ThumbnailUrl160, p.ThumbnailUrl180, p.ThumbnailUrl220, p.ThumbnailUrl310, p.SaleStatus, p.DisplaySequence, p.MainCategoryPath, 
                      p.ExtendCategoryPath, p.SaleCounts, p.ShowSaleCounts, p.AddedDate, p.VistiCounts,
                          (SELECT     COUNT(*) AS Expr1
                            FROM          dbo.Taobao_Products
                            WHERE      (ProductId = p.ProductId)) AS IsMakeTaobao, SKU.SkuId, SKU.SKU, SKU.Weight, SKU.Stock, SKU.CostPrice, SKU.SalePrice, ap.Date, ap.UserId
FROM         dbo.Hishop_Products AS p INNER JOIN
                      dbo.Hishop_SKUs AS SKU ON p.ProductId = SKU.ProductId INNER JOIN
                      dbo.Erp_AgentProduct AS ap ON SKU.SkuId = ap.SkuId
GO