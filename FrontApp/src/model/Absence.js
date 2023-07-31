import Validate from "src/utilities/Validate";

export default class Absence {
  constructor(absence = null) {
    if (!absence) return;

    this.index = absence.index ? absence.index : 0;
    this.matriculation = absence.matriculation ? absence.matriculation : "";
    this.situation = absence.situation ? absence.situation : "";
    this.startDate = absence.startDate ? Validate.adjustDate(absence.startDate) : "";
    this.endDate = absence.endDate ? Validate.adjustDate(absence.endDate) : "";
    this.wantToAssociate = absence.wantToAssociate ? absence.wantToAssociate : "";
    this.entry = absence.entry ? Validate.adjustHour(absence.entry) : "";
    this.departure = absence.departure ? Validate.adjustHour(absence.departure) : "";
    this.note = absence.note ? absence.note : "";
  }
}
