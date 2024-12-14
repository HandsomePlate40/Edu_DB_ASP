--------------------------------------// Admin Proc:

-------------------------------------------// --1
go
CREATE PROCEDURE ViewInfo   
    @LearnerID INT
AS
BEGIN
    SELECT *
    FROM Learner
    WHERE LearnerID = @LearnerID;
END;
go


-------------------------------------------// --2
GO
CREATE PROCEDURE LearnerInfo
    @LearnerID INT
AS
BEGIN
    SELECT *
    FROM PersonalizationProfile
    WHERE LearnerID = @LearnerID;
END;

EXEC LearnerInfo @LearnerID = 1;

-------------------------------------------// --3
GO
CREATE PROCEDURE EmotionalState
    @LearnerID INT,
    @emotional_state VARCHAR(50) OUTPUT
AS
BEGIN
    SELECT TOP 1 @emotional_state = EmotionalState
    FROM EmotionalFeedback
    WHERE LearnerID = @LearnerID
    ORDER BY Timestamp DESC;

    IF @emotional_state IS NULL
    BEGIN
        SET @emotional_state = 'No emotional feedback found';
    END
END
GO

DECLARE @emotional_state VARCHAR(50);
EXEC EmotionalState @LearnerID = 1, @emotional_state = @emotional_state OUTPUT;
PRINT @emotional_state;

-------------------------------------------// --4
GO
CREATE PROCEDURE LogDetails
    @LearnerID INT
AS
BEGIN
    SELECT *
    FROM InteractionLog
    WHERE LearnerID = @LearnerID
    ORDER BY Timestamp DESC;
END;

EXEC LogDetails @LearnerID = 1

-------------------------------------------// --5
go
CREATE PROCEDURE InstructorReview
    @InstructorID INT
AS
BEGIN
    SELECT EF.*
    FROM EmotionalFeedback EF
    INNER JOIN Review R ON EF.FeedbackID = R.FeedbackID
    WHERE R.InstructorID = @InstructorID;
END;
go

EXEC InstructorReview @InstructorID = 5

-------------------------------------------// --6
GO
CREATE PROCEDURE CourseRemove
    @courseID INT
AS
BEGIN
    DELETE FROM Courses
    WHERE CourseID = @courseID;
END
GO

EXEC CourseRemove @CourseID = 19;

-------------------------------------------// --7 
GO
CREATE PROCEDURE Highestgrade
AS
BEGIN
    SELECT 
        a.AssessmentID,
        a.Title,
        a.MaxScore,
        m.CourseID
    FROM 
        Assessments a
    JOIN 
        Modules m ON a.ModuleID = m.ModuleID
    WHERE 
        a.MaxScore = (
            SELECT MAX(MaxScore)
            FROM Assessments
            WHERE ModuleID IN (
                SELECT ModuleID
                FROM Modules
                WHERE CourseID = m.CourseID
            )
        )
    ORDER BY 
        m.CourseID;
END;

EXEC Highestgrade

-------------------------------------------// --8
GO
CREATE PROCEDURE InstructorCount
AS
BEGIN
    SELECT 
        t.CourseID, 
        COUNT(t.InstructorID) AS InstructorCount
    FROM 
        Teach t
    GROUP BY 
        t.CourseID
    HAVING 
        COUNT(t.InstructorID) > 1;
END;


EXEC InstructorCount

-------------------------------------------// --9
go
CREATE PROCEDURE ViewNot
    @LearnerID INT
AS
BEGIN
    SELECT 
        n.NotificationID,
        n.MessageBody,
        n.Timestamp,
        n.UrgencyLevel
    FROM 
        Notifications n
    JOIN 
        Receives r ON n.NotificationID = r.NotificationID
    WHERE 
        r.LearnerID = @LearnerID;
END;
go


EXEC ViewNot @LearnerID = 1

-------------------------------------------// --10 

go
CREATE PROCEDURE CreateDiscussion
    @ModuleID INT,
    @CourseID INT,
    @Title VARCHAR(50),
    @Description VARCHAR(500)
AS
BEGIN
    
    DECLARE @ConfirmationMessage VARCHAR(100);

    IF EXISTS (SELECT 1 FROM Modules WHERE ModuleID = @ModuleID AND CourseID = @CourseID)
    BEGIN

        INSERT INTO DiscussionForum (ForumID, Title, Description, LastActiveTimestamp, ModuleID)
        VALUES (
            (SELECT ISNULL(MAX(ForumID), 0) + 1 FROM DiscussionForum),
            @Title,
            @Description,
            GETDATE(),
            @ModuleID
        );

        SET @ConfirmationMessage = 'Discussion forum created successfully.';
    END
    ELSE
    BEGIN
        SET @ConfirmationMessage = 'Error: Invalid ModuleID or CourseID.';
    END

    PRINT @ConfirmationMessage ;
END;
go

go
EXEC CreateDiscussion 
    @ModuleID = 2, 
    @CourseID = 2, 
    @Title = 'Introduction to TortureSQL', 
    @Description = 'Discuss SQL basics and queries.';
go
-------------------------------------------// --11
go
CREATE PROCEDURE RemoveBadge
    @BadgeID INT
AS
BEGIN

    DECLARE @Confirmation VARCHAR(50);
    DELETE FROM Badges
    WHERE BadgeID = @BadgeID;

    IF @@ROWCOUNT > 0
    BEGIN
        SET @Confirmation = 'Badge successfully removed';
        PRINT @Confirmation;
    END
    ELSE
    BEGIN
        SET @Confirmation = 'Badge not found';
        PRINT @Confirmation;
    END
END
go

EXEC RemoveBadge @BadgeID = 9;

-------------------------------------------// --12
GO
CREATE PROCEDURE CriteriaDelete
    @Criteria VARCHAR(255)
AS
BEGIN
    DELETE FROM Quests
    WHERE Criteria = @Criteria
END;
go


EXEC CriteriaDelete @Criteria = 'HELLO';

-------------------------------------------// --13 --To be tested
GO
CREATE PROCEDURE NotificationUpdate
    @LearnerID INT,
    @NotificationID INT,
    @ReadStatus BIT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM Receives r
        WHERE r.LearnerID = @LearnerID AND r.NotificationID = @NotificationID
    )
    BEGIN
        IF @ReadStatus = 1
        BEGIN
            UPDATE Notifications
            SET ReadStatus = 1
            WHERE NotificationID = @NotificationID;
        END
        ELSE
        BEGIN
            DELETE FROM Notifications
            WHERE NotificationID = @NotificationID;

            DELETE FROM Receives
            WHERE NotificationID = @NotificationID AND LearnerID = @LearnerID;
        END
    END
    ELSE
    BEGIN
        PRINT 'Notification does not exist for the given learner.';
    END
END;
GO

GO
EXEC NotificationUpdate @LearnerID = 1, @NotificationID = 1, @ReadStatus = 1;
GO
-------------------------------------------// --14
GO
CREATE PROCEDURE EmotionalTrendAnalysis
    @CourseID INT,
    @ModuleID INT,
    @TimePeriod DATETIME
AS
BEGIN
    SELECT 
        EF.LearnerID,
        EF.EmotionalState,
        COUNT(EF.FeedbackID) AS FeedbackCount,
        MIN(EF.Timestamp) AS FirstFeedbackTime,
        MAX(EF.Timestamp) AS LastFeedbackTime

    FROM EmotionalFeedback EF
    JOIN Expresses E ON EF.FeedbackID = E.FeedbackID
    JOIN InteractionLog IL ON IL.LearnerID = E.LearnerID AND IL.ActivityID = E.ActivityID
    JOIN LearningActivity LA ON IL.ActivityID = LA.ActivityID
    JOIN Modules M ON LA.ModuleID = M.ModuleID
    JOIN Courses C ON M.CourseID = C.CourseID

    WHERE C.CourseID = @CourseID
      AND M.ModuleID = @ModuleID
      AND EF.Timestamp >= @TimePeriod

    GROUP BY EF.LearnerID, EF.EmotionalState

    ORDER BY EF.LearnerID, EF.EmotionalState;
END;
GO

GO 
EXEC EmotionalTrendAnalysis @CourseID = 1, @ModuleID = 1, @TimePeriod = '2024-01-01 00:00:00';
GO
-------------------------------------------// --1 --Learner
GO
CREATE PROCEDURE ProfileUpdate
    @LearnerID INT,
    @ProfileID INT,
    @PreferredContentType VARCHAR(50),
    @EmotionalState VARCHAR(50),
    @PersonalityType VARCHAR(50)
AS
BEGIN
    UPDATE PersonalizationProfile
    SET 
        PreferredContentTypes = @PreferredContentType,
        EmotionalState = @EmotionalState,
        PersonalityType = @PersonalityType
    WHERE LearnerID = @LearnerID
      AND CreationOrder = @ProfileID;
END;
GO

GO
EXEC ProfileUpdate 
    @LearnerID = 1, 
    @ProfileID = 1, 
    @PreferredContentType = 'AHHHHHHHHHHHHHHHHHH',
    @EmotionalState = 'AHHHHHHHHHHHHHHHHHH',
    @PersonalityType = 'AHHHHHHHHHHHHHHHHHH';
GO   
-------------------------------------------// --2
GO
CREATE PROCEDURE TotalPoints
    @LearnerID INT,
    @RewardType VARCHAR(50)
AS
BEGIN
    DECLARE @TotalPoints DECIMAL(10, 2);

    SELECT @TotalPoints = SUM(R.RewardValue)
    FROM Reward R
    JOIN Grants G ON R.RewardID = G.RewardID
    JOIN Partake P ON G.QuestID = P.QuestID
    WHERE P.LearnerID = @LearnerID
      AND R.RewardType = @RewardType;

    SELECT ISNULL(@TotalPoints, 0) AS TotalPoints;
END;
GO

GO
EXEC TotalPoints 
    @LearnerID = 1, 
    @RewardType = 'Points';
GO
-------------------------------------------// --3
GO
CREATE PROCEDURE EnrolledCourses
    @LearnerID INT
AS
BEGIN
    SELECT CE.CourseID, C.Title, C.CourseDescription, C.DifficultyLevel, C.CreditPoints
    FROM CourseEnrollment CE
    JOIN Courses C ON CE.CourseID = C.CourseID
    WHERE CE.LearnerID = @LearnerID;
END
GO

GO
EXEC EnrolledCourses @LearnerID = 1;
GO
-------------------------------------------// --4  
CREATE PROCEDURE Prerequisites
    @LearnerID INT,
    @CourseID INT
AS
BEGIN
    DECLARE @Prerequisites VARCHAR(212);
    DECLARE @CompletedPrerequisites INT;
    DECLARE @TotalPrerequisites INT;

    SELECT @Prerequisites = Prerequisites FROM Courses WHERE CourseID = @CourseID;

    IF @Prerequisites IS NULL OR @Prerequisites = ''
    BEGIN
        SELECT 'All prerequisites are completed.' AS Message;
        RETURN;
    END

    DECLARE @PrerequisiteList TABLE (Skill VARCHAR(50));
    
    INSERT INTO @PrerequisiteList (Skill)
    SELECT TRIM(value) FROM STRING_SPLIT(@Prerequisites, ',');

    SELECT @CompletedPrerequisites = COUNT(DISTINCT LS.skill)
    FROM LearnerSkills LS
    JOIN @PrerequisiteList PL ON LS.skill = PL.Skill
    WHERE LS.LearnerID = @LearnerID;

    SELECT @TotalPrerequisites = COUNT(*) FROM @PrerequisiteList;

    IF @CompletedPrerequisites = @TotalPrerequisites
        SELECT 'All prerequisites are completed.' AS Message;
    ELSE
        SELECT 'Not all prerequisites are completed.' AS Message;
END;

EXEC Prerequisites @LearnerID = 2, @CourseID = 3;

-------------------------------------------// --5
GO
CREATE PROCEDURE ModuleTraits
    @TargetTrait VARCHAR(50),
    @CourseID INT
AS
BEGIN
    SELECT DISTINCT m.ModuleID, m.Title, m.ContentType, m.ContentURL, m.ModuleDifficulty
    FROM Modules m
    INNER JOIN TargetTraits tt ON m.ModuleID = tt.ModuleID
    WHERE tt.Trait = @TargetTrait AND m.CourseID = @CourseID;
END;
GO

EXEC Moduletraits @TargetTrait = 'Problem Solving', @CourseID = 1;
-------------------------------------------// --6
GO
CREATE PROCEDURE LeaderboardRank
    @LeaderboardID INT
AS
BEGIN
   
    SELECT lr.LeaderboardRank, l.LearnerID, l.first_name, l.last_name
    FROM LeaderboardRanks lr
    INNER JOIN Learner l ON lr.LearnerID = l.LearnerID
    WHERE lr.LeaderboardID = @LeaderboardID
    ORDER BY lr.LeaderboardRank;
END;
GO

EXEC LeaderboardRank @LeaderboardID = 1;
-------------------------------------------// --7
GO
CREATE PROCEDURE ActivityEmotionalFeedback
    @ActivityID INT,
    @LearnerID INT,
    @timestamp DATETIME,
    @emotionalstate VARCHAR(50)
AS
BEGIN
    INSERT INTO EmotionalFeedback (LearnerID, Timestamp, EmotionalState)
    VALUES (@LearnerID, @timestamp, @emotionalstate);

    DECLARE @FeedbackID INT = (SELECT SCOPE_IDENTITY());

    INSERT INTO Expresses (LearnerID, ActivityID, FeedbackID)
    VALUES (@LearnerID, @ActivityID, @FeedbackID);
END;
GO

EXEC ActivityEmotionalFeedback @ActivityID = 5, @LearnerID = 1, @Timestamp = '08:30:00', @emotionalstate = ' mad';
-------------------------------------------// --8
GO
CREATE PROCEDURE JoinQuest
    @LearnerID INT,
    @QuestID INT
AS
BEGIN
    DECLARE @MaxParticipants INT;
    DECLARE @CurrentParticipants INT;

    SELECT @MaxParticipants = Max_Participants
    FROM Collaborative_Quests
    WHERE QuestID = @QuestID;

    SELECT @CurrentParticipants = COUNT(*)
    FROM Partake
    WHERE QuestID = @QuestID;

    IF @CurrentParticipants < @MaxParticipants
    BEGIN
        INSERT INTO Partake (QuestID, LearnerID)
        VALUES (@QuestID, @LearnerID);
        PRINT 'You have successfully joined the quest.';
    END
    ELSE
    BEGIN
        PRINT 'Sorry, the quest is full and cannot accept more participants.';
    END
END;
GO

EXEC JoinQuest @LearnerID = 20, @QuestID = 10;
-------------------------------------------// --9
GO
CREATE PROCEDURE SkillsProfeciency
    @LearnerID INT
AS
BEGIN
   
    SELECT Specific_Skill, ProficiencyLevel
    FROM SkillProgression
    WHERE LearnerID = @LearnerID;
END;
GO

EXEC SkillsProfeciency @LearnerID = 1;
-------------------------------------------// --10                    
GO
CREATE PROCEDURE Viewscore
    @LearnerID INT,
    @AssessmentID INT,
    @score OUTPUT  
AS
BEGIN
    
    SELECT @score = ScoredPoints 
    FROM TakenAssessment 
    WHERE LearnerID = @LearnerID AND AssessmentID = @AssessmentID;

    IF @score IS NULL
    BEGIN
        SET @score = -1;  
    END
    PRINT @score
END;
GO

DECLARE @score INT;
EXEC ViewScore 
    @LearnerID = 101,  
    @AssessmentID = 1,  
    @score = @score OUTPUT;


-------------------------------------------// --11
GO
CREATE PROCEDURE AssessmentsList
    @courseID INT,
    @ModuleID INT,
    @LearnerID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT A.AssessmentID, A.Title, A.AssessmentDescription, TA.ScoredPoints, A.TotalMarks
    FROM Assessments A
    JOIN TakenAssessment TA ON A.AssessmentID = TA.AssessmentID
    JOIN Modules M ON A.ModuleID = M.ModuleID
    JOIN Courses C ON M.CourseID = C.CourseID
    WHERE C.CourseID = @courseID
      AND M.ModuleID = @ModuleID
      AND TA.LearnerID = @LearnerID;
END
GO

EXEC AssessmentsList @CourseID = 1, @ModuleID = 1, @LearnerID = 1;
-------------------------------------------// --12
GO
CREATE PROCEDURE CourseRegister
    @LearnerID INT,
    @CourseID INT
AS
BEGIN
    DECLARE @Prerequisites VARCHAR(212);
    DECLARE @CompletedCourses TABLE (CourseID INT);
    DECLARE @CanRegister BIT = 1;

    SELECT @Prerequisites = Prerequisites
    FROM Courses
    WHERE CourseID = @CourseID;

    INSERT INTO @CompletedCourses
    SELECT CourseID
    FROM CourseEnrollment
    WHERE LearnerID = @LearnerID AND EnrollmentStatus = 'Completed';

    IF @Prerequisites IS NOT NULL AND @Prerequisites <> '' AND @Prerequisites <> 'None'
    BEGIN
        DECLARE @PrereqCourseID VARCHAR(50);
        DECLARE @PrereqCourses CURSOR;
        SET @PrereqCourses = CURSOR FOR
            SELECT value
            FROM STRING_SPLIT(@Prerequisites, ',');

        OPEN @PrereqCourses;
        FETCH NEXT FROM @PrereqCourses INTO @PrereqCourseID;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM @CompletedCourses WHERE CourseID = CAST(@PrereqCourseID AS INT))
            BEGIN
                SET @CanRegister = 0;
                BREAK;
            END
            FETCH NEXT FROM @PrereqCourses INTO @PrereqCourseID;
        END;

        CLOSE @PrereqCourses;
        DEALLOCATE @PrereqCourses;
    END

    IF @CanRegister = 1
    BEGIN
        INSERT INTO CourseEnrollment (EnrollmentID, LearnerID, CourseID, EnrollmentStatus, EnrollmentDate)
        VALUES ((SELECT ISNULL(MAX(EnrollmentID), 0) + 1 FROM CourseEnrollment), @LearnerID, @CourseID, 'Enrolled', GETDATE());
        PRINT 'Registration approved: You have been successfully enrolled in the course.';
    END
    ELSE
    BEGIN
        PRINT 'Registration rejected: Prerequisites for the course are not met.';
    END
END;

EXEC CourseRegister @LearnerID = 1, @CourseID = 1
-----------------------------------------------// --13
GO
CREATE PROCEDURE Post
    @LearnerID INT,
    @DiscussionID INT,
    @Post VARCHAR(MAX)
AS
BEGIN
    BEGIN TRANSACTION

    IF NOT EXISTS (SELECT 1 FROM Learner WHERE LearnerID = @LearnerID)
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR ('LearnerID not found.', 16, 1);
        RETURN;
    END

    IF NOT EXISTS (SELECT 1 FROM DiscussionForum WHERE ForumID = @DiscussionID)
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR ('Discussion Forum not found.', 16, 1);
        RETURN;
    END

    INSERT INTO Joins (LearnerID, ForumID, Post)
    VALUES (@LearnerID, @DiscussionID, @Post);

    COMMIT TRANSACTION
END
GO

EXEC Post @LearnerID = 1, @DiscussionID = 2, @Post = 'Hello AAAAAAAAAAAAAAAAAAAA';
-----------------------------------------------// --14
GO
CREATE PROCEDURE AddGoal
    @LearnerID INT,
    @GoalID INT
AS
BEGIN

    INSERT INTO LearningGoal (GoalID, LearnerID, ObjectiveType, ProgressStatus, GoalDescription, TimeBound)
    VALUES (@GoalID, @LearnerID, 'New Objective', 'Not Started', 'Description of the goal', 0);

END;
GO

EXEC AddGoal @LearnerID = 1, @GoalID = 21;
-----------------------------------------------// --15
GO
CREATE PROCEDURE CurrentPath
    @LearnerID INT
AS
BEGIN
    SELECT 
        LP.PathID,
        LP.LearningPathDescription,
        LP.CompletionStatus
    FROM 
        PersonalizationProfile PP
    INNER JOIN 
        LearningPath LP ON PP.PathID = LP.PathID
    WHERE 
        PP.LearnerID = @LearnerID;
END;
GO

EXEC CurrentPath @LearnerID = 1;
-----------------------------------------------// --16
GO
CREATE PROCEDURE QuestMembers
    @LearnerID INT
AS
BEGIN

    SELECT CQ.QuestID, Q.Title AS QuestTitle, L.LearnerID, L.first_name, L.last_name
    FROM Collaborative_Quests CQ
    JOIN Partake P ON CQ.QuestID = P.QuestID
    JOIN Learner L ON P.LearnerID = L.LearnerID
    JOIN Quests Q ON CQ.QuestID = Q.QuestID
    WHERE CQ.DeadLine > GETDATE()
      AND CQ.QuestID IN (
          SELECT QuestID
          FROM Partake
          WHERE LearnerID = @LearnerID
      )
    ORDER BY CQ.QuestID, L.LearnerID;
END
GO

EXEC QuestMembers @LearnerID = 2;
-----------------------------------------------// --17
GO
CREATE PROCEDURE QuestProgress
    @LearnerID INT
AS
BEGIN
    SELECT 
        Q.QuestID,
        Q.Title AS QuestTitle,
        Q.QuestDescription,
        CASE 
            WHEN P.LearnerID IS NOT NULL THEN 'In Progress'
            ELSE 'Not Started'
        END AS QuestStatus,
        B.BadgeID,
        B.BadgeTitle,
        B.BadgeDescription,
        CASE 
            WHEN T.LearnerID IS NOT NULL THEN 'Earned'
            ELSE 'Not Earned'
        END AS BadgeStatus
    FROM 
        Quests Q
    LEFT JOIN 
        Partake P ON Q.QuestID = P.QuestID AND P.LearnerID = @LearnerID
    LEFT JOIN 
        Badges B ON B.BadgeID = Q.QuestID
    LEFT JOIN 
        Takes T ON B.BadgeID = T.BadgeID AND T.LearnerID = @LearnerID
    WHERE 
        P.LearnerID IS NOT NULL OR T.LearnerID IS NOT NULL;
END;
GO

EXEC QuestProgress @LearnerID = 1;
-----------------------------------------------// --18 
GO
CREATE PROCEDURE GoalReminder
    @LearnerID INT
AS
BEGIN
    DECLARE @GoalID INT;
    DECLARE @GoalDescription VARCHAR(500);
    DECLARE @TimeBound DATETIME;
    DECLARE @CurrentDate DATETIME;
    DECLARE @NotificationMessage VARCHAR(500);
    
    SET @CurrentDate = GETDATE();
    
    -- Cursor to iterate through each goal of the learner
    DECLARE GoalCursor CURSOR FOR
    SELECT GoalID, GoalDescription, TimeBound
    FROM LearningGoal
    WHERE LearnerID = @LearnerID AND ProgressStatus <> 'Completed';
    
    OPEN GoalCursor;
    FETCH NEXT FROM GoalCursor INTO @GoalID, @GoalDescription, @TimeBound;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Check if the current date is past the time bound and if the goal is not completed
        IF (@TimeBound < @CurrentDate)
        BEGIN
            -- Create the notification message
            SET @NotificationMessage = 'Reminder: You are falling behind on your learning goal: ' + @GoalDescription + '. The deadline was on ' + CONVERT(VARCHAR, @TimeBound, 101) + '. Please take necessary actions to get back on track.';
            
            -- Output the notification message to the caller
            PRINT @NotificationMessage;
        END
        ELSE
        BEGIN
            -- Output message indicating the learner is on time
            PRINT 'You are on track with your learning goal: ' + @GoalDescription + '. Keep up the good work!';
        END
        
        FETCH NEXT FROM GoalCursor INTO @GoalID, @GoalDescription, @TimeBound;
    END
    
    CLOSE GoalCursor;
    DEALLOCATE GoalCursor;
END;
GO

EXEC GoalReminder @LearnerID = 6;
-----------------------------------------------// --19  
go
CREATE PROCEDURE SkillProgressHistory
    @LearnerID INT,
    @Skill VARCHAR(50)
AS
BEGIN
    SELECT
        sp.ProgressID,
        sp.Specific_Skill,
        sp.ProficiencyLevel,
        sp.LearnerID
    FROM
        SkillProgression sp
    WHERE
        sp.LearnerID = @LearnerID
        AND sp.Specific_Skill = @Skill
    ORDER BY
        sp.ProgressID;

END
go

EXEC SkillProgressHistory @LearnerID = 9, @Skill = 'Photography';

-----------------------------------------------// --20  --REREVIEW
GO
CREATE PROCEDURE AssessmentAnalysis
    @LearnerID INT
AS
BEGIN
    SELECT 
        A.AssessmentID,
        A.Title AS AssessmentTitle,
        A.AssessmentDescription,
        A.MaxScore,
        COALESCE(A.TotalMarks, 0) AS Score,
        CASE 
            WHEN A.TotalMarks IS NULL THEN 'Not Attempted'
            WHEN A.TotalMarks >= A.PassingMarks THEN 'Passed'
            ELSE 'Failed'
        END AS PerformanceStatus
    FROM 
        Assessments A
    LEFT JOIN 
        Achievement Ach ON Ach.LearnerID = @LearnerID AND Ach.BadgeID = A.AssessmentID
    WHERE 
        Ach.LearnerID IS NOT NULL OR A.TotalMarks IS NOT NULL
    ORDER BY 
        A.AssessmentID;
END;
GO

EXEC AssessmentAnalysis @LearnerID = 1;
-----------------------------------------------// --21
GO
CREATE PROCEDURE LeaderboardFilter
    @LearnerID INT
AS
BEGIN
    SELECT 
        LR.LeaderboardID,
        LR.LeaderboardRank,
        L.LearnerID,
        L.first_name + ' ' + L.last_name AS LearnerName,
        LB.TotalPoints,
        LB.Season
    FROM 
        LeaderboardRanks LR
    INNER JOIN 
        Learner L ON LR.LearnerID = L.LearnerID
    INNER JOIN 
        Leaderboard LB ON LR.LeaderboardID = LB.LeaderboardID
    WHERE 
        LR.LearnerID = @LearnerID
    ORDER BY 
        LR.LeaderboardRank DESC;
END;
GO

EXEC LeaderboardFilter @LearnerID = 1;
-----------------------------------------------// --Instructor --1
GO
CREATE PROCEDURE SkillLearners
    @skill VARCHAR(50)
AS
BEGIN
    SELECT 
        LS.skill AS SkillName,
        L.LearnerID,
        L.first_name AS FirstName,
        L.last_name AS LastName
    FROM 
        LearnerSkills LS
    INNER JOIN 
        Learner L ON LS.LearnerID = L.LearnerID
    WHERE 
        LS.skill = @skill;
END;
GO

EXEC SkillLearners @skill = 'JavaScript';
-----------------------------------------------// --2
GO
CREATE PROCEDURE NewActivity
    @CourseID INT,
    @ModuleID INT,
    @activitytype VARCHAR(50),
    @instructiondetails VARCHAR(MAX),
    @maxpoints INT
AS
BEGIN
    DECLARE @NewActivityID INT;

    INSERT INTO LearningActivity (ActivityType, MaxPoints, ModuleID)
    VALUES (@activitytype, @maxpoints, @ModuleID);

    SET @NewActivityID = SCOPE_IDENTITY();

    INSERT INTO ActivityInstructions (ActivityID, Instruction)
    VALUES (@NewActivityID, @instructiondetails);
END;
GO

EXEC NewActivity @CourseID = 1,  
    @ModuleID = 1, 
    @activitytype = 'Quiz',
    @instructiondetails = 'Complete all questions by next week.', 
    @maxpoints = 100; 
-----------------------------------------------// --3
GO
CREATE PROCEDURE NewAchievement
    @LearnerID INT,
    @BadgeID INT,
    @AchievmentDescription VARCHAR(MAX),
    @date_earned DATE,
    @type VARCHAR(50)
AS
BEGIN
    INSERT INTO Achievement ( AchievementType, DateEarned, LearnerID, BadgeID, AchievementDescription)
    VALUES ( @type, @date_earned, @LearnerID, @BadgeID, @AchievmentDescription);
END;
GO

EXEC NewAchievement 
    @LearnerID = 1,
    @BadgeID = 2,      
    @date_earned = '2024-11-24',
    @type = 'Badge',
    @AchievmentDescription = 'AAAAAAAAAAAAAHHHHH'
-----------------------------------------------// --4
GO
CREATE PROCEDURE LearnerBadge
    @BadgeID INT
AS
BEGIN
    SELECT 
        L.LearnerID,
        L.first_name AS FirstName,
        L.last_name AS LastName,
        A.DateEarned
    FROM 
        Achievement A
    INNER JOIN 
        Learner L ON A.LearnerID = L.LearnerID
    WHERE 
        A.BadgeID = @BadgeID;
END;
GO

EXEC LearnerBadge @BadgeID = 1;
-----------------------------------------------// --5
GO
CREATE PROCEDURE NewPath
    @LearnerID INT,
    @ProfileID INT,
    @completion_status VARCHAR(50),
    @custom_content VARCHAR(MAX),
    @adaptiverules VARCHAR(MAX),
    @GoalID INT
AS
BEGIN
    DECLARE @NewPathID INT;

    INSERT INTO LearningPath (CompletionStatus, LearningPathDescription, AdaptiveRules, GoalID)
    VALUES (@completion_status, @custom_content, @adaptiverules, @GoalID);

    SET @NewPathID = SCOPE_IDENTITY();

    INSERT INTO PersonalizationProfile (LearnerID, PathID)
    VALUES (@LearnerID, @NewPathID);
END;
GO

EXEC NewPath 
    @LearnerID = 1,                
    @ProfileID = 1, 
    @completion_status = 'In Progress', 
    @custom_content = 'Introduction to Data Science', 
    @adaptiverules = 'Rule1; Rule2',  
    @goalID = 1;                 
-----------------------------------------------// --6
GO
CREATE PROCEDURE TakenCourses
    @LearnerID INT
AS
BEGIN
    SELECT DISTINCT
        c.CourseID,
        c.Title
    FROM
        CourseEnrollment e
    INNER JOIN
        Courses c
        ON e.CourseID = c.CourseID
    WHERE
        e.LearnerID = @LearnerID
    ORDER BY
        c.Title;
END;
GO

EXEC TakenCourses @LearnerID = 2;
-----------------------------------------------// --7
GO
CREATE PROCEDURE CollaborativeQuest
    @difficulty_level VARCHAR(50),
    @criteria VARCHAR(50),
    @description VARCHAR(50),
    @title VARCHAR(50),
    @Maxnumparticipants INT,
    @deadline DATETIME
AS
BEGIN

    DECLARE @NewQuestID INT;

    INSERT INTO Quests (Title, QuestDescription, DifficultyLevel,QuestType ,Criteria)
    VALUES (@title, @description, @difficulty_level,'Collaborative_Quest', @criteria);

    SET @NewQuestID = SCOPE_IDENTITY();

    INSERT INTO Collaborative_Quests (QuestID, Max_Participants, DeadLine)
    VALUES (@NewQuestID, @Maxnumparticipants, @deadline);
END;
GO

EXEC CollaborativeQuest 
    @difficulty_level = 'Medium',
    @criteria = 'Complete all tasks',
    @description = 'A collaborative project on data science.',
    @title = 'Data Science Collaboration',
    @Maxnumparticipants = 10,
    @deadline = '2024-12-31 23:59:59';
-----------------------------------------------// --8
GO
CREATE PROCEDURE DeadlineUpdate
    @QuestID INT,
    @deadline DATETIME
AS
BEGIN
    UPDATE Collaborative_Quests
    SET DeadLine = @deadline
    WHERE QuestID = @QuestID;
END;
GO

EXEC DeadlineUpdate 
    @QuestID = 3,
    @deadline = '2024-12-31 23:59:59';
-----------------------------------------------// --9
GO
CREATE PROCEDURE GradeUpdate
    @LearnerID INT,
    @AssessmentID INT,
    @points INT
AS
BEGIN
    UPDATE TakenAssessment
    SET ScoredPoints = @points
    WHERE LearnerID = @LearnerID AND AssessmentID = @AssessmentID;

    PRINT 'Grade updated successfully';
END;
GO

EXEC GradeUpdate 
    @LearnerID = 1,
    @AssessmentID = 1,
    @points = 90;
-----------------------------------------------// --10
GO
CREATE PROCEDURE AssessmentNot
    @NotificationID INT,
    @timestamp DATETIME,
    @message VARCHAR(MAX),
    @urgencylevel VARCHAR(50),
    @LearnerID INT
AS
BEGIN
    SET IDENTITY_INSERT Notifications ON; 
    INSERT INTO Notifications (NotificationID, MessageBody, Timestamp, UrgencyLevel, ReadStatus)
    VALUES (@NotificationID, @message, @timestamp, @urgencylevel, 0);
    SET IDENTITY_INSERT Notifications OFF; 

    INSERT INTO Receives (LearnerID, NotificationID)
    VALUES (@LearnerID, @NotificationID);

    PRINT 'Notification successfully sent to the learner regarding the upcoming assessment deadline.'
END;
GO

EXEC AssessmentNot
    @NotificationID = 30,
    @timestamp = '11/22/2024',
    @message = 'I hate myself',
    @urgencylevel = 'Urgent',
    @LearnerID = 40;
-----------------------------------------------// --11 
GO
CREATE PROCEDURE NewGoal
    @GoalID INT,
    @status VARCHAR(MAX),
    @deadline DATETIME,
    @description VARCHAR(MAX)
AS
BEGIN
    INSERT INTO LearningGoal (GoalID, ProgressStatus, GoalDescription, TimeBound)
    VALUES (@GoalID, @status, @description, @deadline);
END

EXEC NewGoal
    @GoalID = 100,
    @status = 'In Progress',
    @deadline = '2024-12-31 23:59:59',
    @description = 'Complete the advanced SQL course for mastering database management.';
------------------------------------------------// --12
  GO
CREATE PROCEDURE LearnersCourses
    @CourseID INT,
    @InstructorID INT
AS
BEGIN
    SELECT 
        c.Title,
        l.LearnerID,
        l.first_name,
        l.last_name,
        e.EnrollmentDate
    FROM 
        CourseEnrollment e
    JOIN 
        Learner l ON e.LearnerID = l.LearnerID
    JOIN 
        Courses c ON e.CourseID = c.CourseID
    JOIN 
        Teach t ON c.CourseID = t.CourseID
    WHERE 
        c.CourseID = @CourseID
        AND t.InstructorID = @InstructorID;
END;



EXEC LearnersCourses
    @CourseID = 1,
    @InstructorID = 1;

 -----------------------------------------------// --13
 go
  CREATE PROCEDURE LastActive
    @ForumID INT,
    @lastactive DATETIME OUTPUT
AS
BEGIN

    SELECT @lastactive = LastActiveTimestamp
    FROM DiscussionForum
    WHERE ForumID = @ForumID;

    IF @lastactive IS NULL
    BEGIN
        PRINT 'No active record found for the specified ForumID.';
    END
END;


DECLARE @lastActiveTime DATETIME;
EXEC LastActive 
    @ForumID = 1,
    @lastactive = @lastActiveTime OUTPUT;

SELECT @lastActiveTime AS LastActiveTimestamp;
 -----------------------------------------------// 14
 GO
 CREATE PROCEDURE CommonEmotionalState
    @state VARCHAR(50) OUTPUT
AS
BEGIN

    DECLARE @MostCommonState VARCHAR(50);

    SELECT TOP 1 
        @MostCommonState = EmotionalState
    FROM 
        EmotionalFeedback
    GROUP BY 
        EmotionalState
    ORDER BY 
        COUNT(*) DESC;

    SET @state = @MostCommonState;

    IF @state IS NULL
    BEGIN
        SET @state = 'No emotional state recorded.';
    END
END;

DECLARE @commonState VARCHAR(50);
EXEC CommonEmotionalState @state = @commonState OUTPUT;
SELECT @commonState AS MostCommonEmotionalState;
 -----------------------------------------------// --15
GO
CREATE PROCEDURE ModuleDifficulty
    @courseID INT
AS
BEGIN
    SELECT ModuleID, Title, ContentType, ContentURL, ModuleDifficulty
    FROM Modules
    WHERE CourseID = @courseID
    ORDER BY ModuleDifficulty;
END
GO

EXEC ModuleDifficulty @CourseID = 1;

-----------------------------------------------// --16
go
CREATE PROCEDURE Profeciencylevel
    @LearnerID INT,
    @skill VARCHAR(50) OUTPUT
AS
BEGIN
    
    DECLARE @HighestProficiencyLevel VARCHAR(50);

   
    SELECT TOP 1 
        @skill = Specific_Skill
    FROM 
        SkillProgression
    WHERE 
        LearnerID = @LearnerID
    ORDER BY 
        ProficiencyLevel DESC;  

   
    IF @skill IS NULL
    BEGIN
        SET @skill = 'No skills recorded for this learner.';
    END
END;


DECLARE @highestSkill VARCHAR(50);
EXEC Profeciencylevel 
    @LearnerID = 1,
    @skill = @highestSkill OUTPUT;
SELECT @highestSkill AS HighestProficiencySkill;

-----------------------------------------------// --17
GO
CREATE PROCEDURE ProfeciencyUpdate
    @Skill VARCHAR(50),
    @LearnerId INT,
    @Level VARCHAR(50)
AS
BEGIN
  
    UPDATE SkillProgression
    SET ProficiencyLevel = @Level
    WHERE Specific_Skill = @Skill AND LearnerID = @LearnerId;
END;

EXEC ProfeciencyUpdate 
    @Skill = 'Data Analysis',   
    @LearnerId = 1,             
    @Level = 'Advanced';       
-----------------------------------------------// --18
GO
CREATE PROCEDURE LeastBadge
    @LearnerID INT OUTPUT
AS
BEGIN
    SELECT TOP 1 
        @LearnerID = LearnerID
    FROM 
        Takes
    GROUP BY 
        LearnerID
    ORDER BY 
        COUNT(BadgeID) ASC;

    IF @LearnerID IS NULL
    BEGIN
        PRINT 'No learners found with badges.';
    END
END;

DECLARE @learnerWithLeastBadges INT;
EXEC LeastBadge @LearnerID = @learnerWithLeastBadges OUTPUT;
SELECT @learnerWithLeastBadges AS LearnerWithLeastBadges;

-----------------------------------------------// --19
GO
CREATE PROCEDURE PreferedType
    @type VARCHAR(50) OUTPUT
AS
BEGIN
    SELECT TOP 1 
        @type = preference
    FROM 
        LearningPreferences
    GROUP BY 
        preference
    ORDER BY 
        COUNT(*) DESC;

    IF @type IS NULL
    BEGIN
        SET @type = 'No learning preferences recorded.';
    END
END;

DECLARE @mostPreferredType VARCHAR(50);
EXEC PreferedType @type = @mostPreferredType OUTPUT;
SELECT @mostPreferredType AS MostPreferredLearningType;

-----------------------------------------------// --20
GO
CREATE PROCEDURE AssessmentAnalytics
    @CourseID INT,
    @ModuleID INT
AS
BEGIN
    SELECT 
        a.AssessmentID,
        a.Title AS AssessmentTitle,
        AVG(a.TotalMarks) AS AverageScore
    FROM 
        Assessments a
    JOIN 
        CourseEnrollment ce ON ce.CourseID = @CourseID
    JOIN 
        Learner l ON l.LearnerID = ce.LearnerID
    WHERE 
        a.ModuleID = @ModuleID
    GROUP BY 
        a.AssessmentID, a.Title;
END;
GO

EXEC AssessmentAnalytics 
    @CourseID = 1,
    @ModuleID = 2;
-----------------------------------------------// --21
GO
CREATE PROCEDURE EmotionalTrendAnalysis
    @CourseID INT,
    @ModuleID INT,
    @TimePeriod DATETIME
AS
BEGIN
    SELECT 
        ef.LearnerID,
        ef.EmotionalState,
        COUNT(*) AS FeedbackCount,
        MIN(ef.Timestamp) AS FirstFeedbackDate,
        MAX(ef.Timestamp) AS LastFeedbackDate
    FROM 
        EmotionalFeedback ef
    JOIN 
        CourseEnrollment ce ON ef.LearnerID = ce.LearnerID
    JOIN 
        Modules m ON m.ModuleID = @ModuleID
    WHERE 
        ce.CourseID = @CourseID AND 
        ef.Timestamp >= @TimePeriod
    GROUP BY 
        ef.LearnerID, ef.EmotionalState
    ORDER BY 
        FeedbackCount DESC;
END;
GO

EXEC EmotionalTrendAnalysis 
    @CourseID = 10,          
    @ModuleID = 10,         
    @TimePeriod = '1/25/2024 12:10:00 PM'; 
