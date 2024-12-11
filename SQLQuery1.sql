-- Add Email and PasswordHash columns to Learners
ALTER TABLE Learner
ADD Email NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL;

-- Add Email and PasswordHash columns to Instructors
ALTER TABLE Instructor
ADD 
    PasswordHash NVARCHAR(255) NOT NULL;

-- Drop foreign key constraints from Learners and Instructors
ALTER TABLE Learner DROP CONSTRAINT FK_Learners_Users;
ALTER TABLE Instructor DROP CONSTRAINT FK_Instructor_User;

-- Optional: Drop the Users table if it's no longer needed
DROP TABLE Users;
