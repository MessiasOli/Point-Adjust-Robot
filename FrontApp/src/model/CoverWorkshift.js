import Validate from "src/utilities/Validate"

export default class CoverWorkshift {
  constructor(data = {}){
    this.index = data.index ? data.index : "";
    this.operationType = data.operationType ? data.operationType : "";
    this.matriculation = data.matriculation ? data.matriculation : "";
    this.client = data.client ? data.client : "";
    this.place = data.place ? data.place : "";
    this.reason = data.reason ? data.reason : "";
    this.hedgingFeature = data.hedgingFeature ? data.hedgingFeature : "";
    this.startDate = data.startDate ? Validate.adjustDate(data.startDate) : "";
    this.endDate = data.endDate ? Validate.adjustDate(data.endDate) : "";
    this.enterTimeManually = data.enterTimeManually ? data.enterTimeManually : "";
    this.postCalculationProfile = data.postCalculationProfile ? data.postCalculationProfile : "";
    this.employeeHours = data.employeeHours ? data.employeeHours : "";
    this.entry1 = data.entry1 ? Validate.adjustHour(data.entry1) : "";
    this.departure1 = data.departure1 ? Validate.adjustHour(data.departure1) : "";
    this.entry2 = data.entry2 ? Validate.adjustHour(data.entry2) : "";
    this.departure2 = data.departure2 ? Validate.adjustHour(data.departure2) : "";
    this.description = data.description ? data.description : "";
  }
}
