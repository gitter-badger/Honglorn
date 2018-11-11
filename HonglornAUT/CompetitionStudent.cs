﻿using HonglornBL.Enums;

namespace HonglornAUT
{
    class CompetitionStudent
    {
        internal string Course { get; }
        internal string Forename { get; }
        internal string Surname { get; }
        internal Sex Sex { get; }
        internal float? Sprint { get; }
        internal float? Jump { get; }
        internal float? Throw { get; }
        internal float? MiddleDistance { get; }
        internal ushort SprintScore { get; }
        internal ushort JumpScore { get; }
        internal ushort ThrowScore { get; }
        internal ushort MiddleDistanceScore { get; }

        Certificate Certificate { get; }

        internal CompetitionStudent(string course, string forename, string surname, Sex sex, float? sprint, float? jump, float? @throw, float? middleDistance, ushort sprintScore, ushort jumpScore, ushort throwScore, ushort middleDistanceScore, Certificate certificate)
        {
            Course = course;
            Forename = forename;
            Surname = surname;
            Sex = sex;
            Sprint = sprint;
            Jump = jump;
            Throw = @throw;
            MiddleDistance = middleDistance;
            SprintScore = sprintScore;
            JumpScore = jumpScore;
            ThrowScore = throwScore;
            MiddleDistanceScore = middleDistanceScore;
            Certificate = certificate;
        }

        public override string ToString() => $"{Sex} ({Sprint}|{SprintScore}) ({Jump}|{JumpScore}) ({Throw}|{ThrowScore}) ({MiddleDistance}|{MiddleDistanceScore}) {Certificate}";
    }
}