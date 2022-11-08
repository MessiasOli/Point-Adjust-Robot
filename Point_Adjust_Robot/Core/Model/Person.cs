namespace Point_Adjust_Robot.Core.Model
{
    public class Person
    {
            public int id {get; set;}
            public int personSituationId {get; set;}
            public int personTypeId {get; set;}
            public int careerId {get; set;}
            public int companyId {get; set;}
            public string enrolment {get; set;}
            public string name {get; set;}
            public string gender {get; set;}
            public string birthDate {get; set;}
            public string admissionDate {get; set;}
            public int workplaceId {get; set;}
            public string externalWorkplaceId {get; set;}
            public int scheduleId {get; set;}
            public int rotationId {get; set;}
            public bool ignoreValidation {get; set;}
            public bool allowDevicePassword {get; set;}
            public bool allowMobileClocking {get; set;}
            public string externalCompanyId {get; set;}
            public string externalScheduleId {get; set;}
            public string externalCareerId {get; set;}
            public int rotationCode { get; set;}
    }
}
