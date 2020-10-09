USE [EventsDatabase]
GO
/****** Object:  StoredProcedure [dbo].[spAttendees_Delete]    Script Date: 10/8/2020 3:33:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spAttendees_Delete]
	-- Add the parameters for the stored procedure here
	@attendeeID int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM attendees
	WHERE attendeeID = @attendeeID
END




USE [EventsDatabase]
GO
/****** Object:  StoredProcedure [dbo].[spAttendees_Insert]    Script Date: 10/8/2020 3:34:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spAttendees_Insert]
	-- Add the parameters for the stored procedure here
	@email nvarchar(200) = NULL,
	@rsvp nvarchar(20)
	-- @id int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- SET @id = (SELECT SCOPE_IDENTITY() FROM event);

    -- Insert statements for procedure here
	IF @email is NOT NULL
	BEGIN
		INSERT INTO attendees (eventID, email, rsvp) 
		VALUES ((SELECT MAX(eventID) FROM event), @email, @rsvp);
	END
END




USE [EventsDatabase]
GO
/****** Object:  StoredProcedure [dbo].[spEvent_DeleteEvent]    Script Date: 10/8/2020 3:34:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spEvent_DeleteEvent] 
	@eventID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM event 
	WHERE eventID = @eventID
END




USE [EventsDatabase]
GO
/****** Object:  StoredProcedure [dbo].[spEvent_InsertEvent]    Script Date: 10/8/2020 3:35:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spEvent_InsertEvent] 
	@summary nvarchar(255),
	@description nvarchar(500) = null,
	@startTime dateTime,
	@endTime dateTime,
	@dtstamp dateTime,
	@uniqueIdentifier nvarchar(50),
	@timezone nvarchar(20),
	@classification varchar(20),
	@organizer nvarchar(200) = null,
	@recurFrequency nvarchar(20) = 'Once',
	@recurDateTime dateTime = null,
	@eventLocation nvarchar(200) = null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO event (summary, description, startTime, endTime, dtstamp, uniqueIdentifier, timezone, classification, organizer, recurDateTime, recurFrequency, eventLocation) 
	VALUES (@summary, @description, @startTime, @endTime, @dtstamp, @uniqueIdentifier, @timezone, @classification, @organizer, @recurDateTime, @recurFrequency, @eventLocation);

END




USE [EventsDatabase]
GO
/****** Object:  StoredProcedure [dbo].[spEvent_SelectEvent]    Script Date: 10/8/2020 3:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spEvent_SelectEvent]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	-- SELECT eventID, summary, description, startTime, endTime, uniqueIdentifier, timezone, classification, dtstamp FROM event

	SELECT event.eventID, summary, description, startTime, endTime, uniqueIdentifier, timezone, classification, dtstamp, organizer, attendeeID, email, rsvp, recurFrequency, recurDateTime, eventLocation FROM event
	FULL JOIN attendees
	ON attendees.eventID = event.eventID;

END




USE [EventsDatabase]
GO
/****** Object:  StoredProcedure [dbo].[spEvent_UpdateEvent]    Script Date: 10/8/2020 3:35:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[spEvent_UpdateEvent]
	-- Add the parameters for the stored procedure here
	@eventID int,
	@summary nvarchar(255),
	@description nvarchar(500) = null,
	@startTime dateTime,
	@endTime dateTime,
	@timezone nvarchar(20),
	@classification varchar(20),
	@organizer nvarchar(200) = null,
	@email nvarchar(200) = null,
	@rsvp nvarchar(20) = null,
	@attendeeID int = null,
	@recurDateTime dateTime= null,
	@recurFrequency nvarchar(20) = 'Once',
	@eventLocation nvarchar(200) = null

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	BEGIN TRANSACTION;

	UPDATE event
	SET summary = @summary, description = @description, startTime = @startTime, endTime = @endTime, timezone = @timezone, classification = @classification, organizer = @organizer, recurDateTime = @recurDateTime, recurFrequency = @recurFrequency, eventLocation = @eventLocation
	WHERE eventID = @eventID

	IF @email IS NOT NULL
	BEGIN
		UPDATE attendees
		SET	email = @email, rsvp = @rsvp
		WHERE eventID = @eventID AND attendeeID = @attendeeID
	END
	
	IF NOT EXISTS(SELECT * FROM attendees WHERE email = @email AND eventID = @eventID) AND @email IS NOT NULL
	BEGIN
		INSERT INTO attendees (eventID, email, rsvp)
		VALUES (@eventID, @email, @rsvp)
	END

	COMMIT;
END
