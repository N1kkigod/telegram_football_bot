USE [master]
GO
/****** Object:  Database [telegram_football_bot]    Script Date: 18.06.2021 7:38:06 ******/
CREATE DATABASE [telegram_football_bot]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'telegram_football_bot', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\telegram_football_bot.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'telegram_football_bot_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\telegram_football_bot_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [telegram_football_bot] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [telegram_football_bot].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [telegram_football_bot] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [telegram_football_bot] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [telegram_football_bot] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [telegram_football_bot] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [telegram_football_bot] SET ARITHABORT OFF 
GO
ALTER DATABASE [telegram_football_bot] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [telegram_football_bot] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [telegram_football_bot] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [telegram_football_bot] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [telegram_football_bot] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [telegram_football_bot] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [telegram_football_bot] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [telegram_football_bot] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [telegram_football_bot] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [telegram_football_bot] SET  DISABLE_BROKER 
GO
ALTER DATABASE [telegram_football_bot] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [telegram_football_bot] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [telegram_football_bot] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [telegram_football_bot] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [telegram_football_bot] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [telegram_football_bot] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [telegram_football_bot] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [telegram_football_bot] SET RECOVERY FULL 
GO
ALTER DATABASE [telegram_football_bot] SET  MULTI_USER 
GO
ALTER DATABASE [telegram_football_bot] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [telegram_football_bot] SET DB_CHAINING OFF 
GO
ALTER DATABASE [telegram_football_bot] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [telegram_football_bot] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [telegram_football_bot] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [telegram_football_bot] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'telegram_football_bot', N'ON'
GO
ALTER DATABASE [telegram_football_bot] SET QUERY_STORE = OFF
GO
USE [telegram_football_bot]
GO
/****** Object:  Table [dbo].[Bets]    Script Date: 18.06.2021 7:38:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bets](
	[BetID] [int] NOT NULL,
	[MatchID] [int] NULL,
	[Command] [nvarchar](max) NULL,
	[BetValue] [int] NULL,
	[UserID] [int] NULL,
 CONSTRAINT [PK_Bets] PRIMARY KEY CLUSTERED 
(
	[BetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Matches]    Script Date: 18.06.2021 7:38:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Matches](
	[MatchID] [int] NOT NULL,
	[Date] [nvarchar](max) NULL,
	[TournamentName] [nvarchar](max) NULL,
	[Command1] [nvarchar](max) NULL,
	[Command2] [nvarchar](max) NULL,
	[Command1Score] [int] NULL,
	[Command2Score] [int] NULL,
	[MatchStatus] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tournaments]    Script Date: 18.06.2021 7:38:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tournaments](
	[TournamentID] [int] NOT NULL,
	[TournamentName] [nvarchar](max) NULL,
	[MatchID] [int] NULL,
 CONSTRAINT [PK_Tournaments] PRIMARY KEY CLUSTERED 
(
	[TournamentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 18.06.2021 7:38:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] NOT NULL,
	[UserTelegramID] [int] NULL,
	[Username] [nvarchar](max) NULL,
	[Score] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[Bets] ([BetID], [MatchID], [Command], [BetValue], [UserID]) VALUES (1, 2, N'TestCommand3', 5, 2)
INSERT [dbo].[Bets] ([BetID], [MatchID], [Command], [BetValue], [UserID]) VALUES (2, 1, N'TestCommand1', 5, 2)
INSERT [dbo].[Bets] ([BetID], [MatchID], [Command], [BetValue], [UserID]) VALUES (3, 2, N'TestCommand3', 5, 2)
INSERT [dbo].[Bets] ([BetID], [MatchID], [Command], [BetValue], [UserID]) VALUES (4, 1, N'TestCommand1', 5, 1)
INSERT [dbo].[Bets] ([BetID], [MatchID], [Command], [BetValue], [UserID]) VALUES (5, 1, N'TestCommand2', 5, 1)
INSERT [dbo].[Bets] ([BetID], [MatchID], [Command], [BetValue], [UserID]) VALUES (6, 1, N'TestCommand2', 5, 2)
GO
INSERT [dbo].[Matches] ([MatchID], [Date], [TournamentName], [Command1], [Command2], [Command1Score], [Command2Score], [MatchStatus]) VALUES (1, N'17.06.2021', N'TestTournament', N'TestCommand1', N'TestCommand2', 2, 2, N'Ongoing')
INSERT [dbo].[Matches] ([MatchID], [Date], [TournamentName], [Command1], [Command2], [Command1Score], [Command2Score], [MatchStatus]) VALUES (2, N'17.06.2021', N'TestTournament2', N'TestCommand3', N'TestCommand4', 4, 2, N'Ongoing')
GO
INSERT [dbo].[Tournaments] ([TournamentID], [TournamentName], [MatchID]) VALUES (1, N'TestTournament', 1)
INSERT [dbo].[Tournaments] ([TournamentID], [TournamentName], [MatchID]) VALUES (2, N'TestTournament2', 2)
GO
INSERT [dbo].[Users] ([UserID], [UserTelegramID], [Username], [Score]) VALUES (1, 381645250, N'xabo01', 40)
INSERT [dbo].[Users] ([UserID], [UserTelegramID], [Username], [Score]) VALUES (2, 390699100, N'N1kkigod', 10)
GO
USE [master]
GO
ALTER DATABASE [telegram_football_bot] SET  READ_WRITE 
GO
