CREATE TABLE [dbo].[UserActivation](
	[Id] [uniqueidentifier] NOT NULL,
	[ShortCode] [nvarchar](5) NOT NULL,
	[IdentityCode] [nvarchar](1024) NOT NULL,
	[Created] [datetime] NOT NULL,
	[Expires] [datetime] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_UserActivation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserActivation] ADD  CONSTRAINT [DF_UserActivation_Created]  DEFAULT (getdate()) FOR [Created]