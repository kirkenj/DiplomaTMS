using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table(nameof(MonthReport) + "s")]
    public class MonthReport
    {
        public int ContractID { get; set; }
        public Contract Contract { get; set; } = null!;
        public int Month { get; set; }
        public int Year { get; set; }
        public int LectionsTime { get; set; }
        public int PracticalClassesTime { get; set; }
        public int LaboratoryClassesTime  { get; set; }
        public int ConsultationsTime  { get; set; }
        public int OtherTeachingClassesTime { get; set; }
        public int CreditsTime  { get; set; }
        public int ExamsTime { get; set; }
        public int CourseProjectsTime { get; set; }
        public int InterviewsTime { get; set; }
        public int TestsAndReferatsTime { get; set; }
        public int InternshipsTime { get; set; }
        public int DiplomasTime { get; set; }
        public int DiplomasReviewsTime { get; set; }
        public int SECTime { get; set; }
        public int GraduatesManagementTime { get; set; }
        public int GraduatesAcademicWorkTime { get; set; }
        public int PlasticPosesDemonstrationTime { get; set; }
        public int TestingEscortTime { get; set; }
        public int TimeSum =>
            TestingEscortTime
            + PlasticPosesDemonstrationTime
            + GraduatesAcademicWorkTime
            + GraduatesManagementTime
            + SECTime
            + DiplomasReviewsTime
            + DiplomasTime
            + InternshipsTime
            + TestsAndReferatsTime
            + InterviewsTime
            + LectionsTime
            + PracticalClassesTime
            + LaboratoryClassesTime
            + ConsultationsTime
            + OtherTeachingClassesTime
            + CreditsTime
            + ExamsTime
            + CourseProjectsTime;
    }
}
