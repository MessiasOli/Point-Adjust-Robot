import Validate from "src/utilities/Validate";

export default class AdjustWorkshift {
  constructor(adjust = null){
      if (!adjust) return;

      this.index = adjust.index ? adjust.index : 0;
      this.matriculation = adjust.matriculation ? adjust.matriculation : "";
      this.hour = adjust.hour ? Validate.adjustHour(adjust.hour.trim()) : "";
      this.replaceTime = adjust.replaceTime ? Validate.adjustHour(adjust.replaceTime) : "";
      this.reference = adjust.reference ? Validate.adjustDate(adjust.reference) : "";
      this.date = adjust.date ? Validate.adjustDate(adjust.date) : "";
      this.justification = adjust.justification ? adjust.justification : "";
      this.note = adjust.note ? adjust.note : "";
  }
}
