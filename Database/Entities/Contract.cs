using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities
{
    [Table(nameof(Contract)+"s")]
    public class Contract
    {
        public int ID { get; set; }
        public User User { get; set; } = null!;
        public int UserID { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public bool IsConfirmed { get; set; } = false;
        public IEnumerable<MonthReport> MonthReports { get; set; } = null!;
        public int LectionsMaxTime { get; set; }
        public int PracticalClassesMaxTime { get; set; }
        public int LaboratoryClassesMaxTime { get; set; }
        public int ConsultationsMaxTime { get; set; }
        public int OtherTeachingClassesMaxTime { get; set; }
        public int CreditsMaxTime { get; set; }
        public int ExamsMaxTime { get; set; }
        public int CourseProjectsMaxTime { get; set; }
        public int InterviewsMaxTime { get; set; }
        public int TestsAndReferatsMaxTime { get; set; }
        public int InternshipsMaxTime { get; set; }
        public int DiplomasMaxTime { get; set; }
        public int DiplomasReviewsMaxTime { get; set; }
        public int SECMaxTime { get; set; }
        public int GraduatesManagementMaxTime { get; set; }
        public int GraduatesAcademicWorkMaxTime { get; set; }
        public int PlasticPosesDemonstrationMaxTime { get; set; }
        public int TestingEscortMaxTime { get; set; }

        public int TimeSum =>
            TestingEscortMaxTime
            + PlasticPosesDemonstrationMaxTime
            + GraduatesAcademicWorkMaxTime
            + GraduatesManagementMaxTime
            + SECMaxTime
            + DiplomasReviewsMaxTime
            + DiplomasMaxTime
            + InternshipsMaxTime
            + TestsAndReferatsMaxTime
            + InterviewsMaxTime
            + LectionsMaxTime
            + PracticalClassesMaxTime
            + LaboratoryClassesMaxTime
            + ConsultationsMaxTime
            + OtherTeachingClassesMaxTime
            + CreditsMaxTime
            + ExamsMaxTime
            + CourseProjectsMaxTime;
    }
}