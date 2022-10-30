import Command from "./Command";

export default class CommandAdjust extends Command {
  constructor(workShiftAdjustments = Array) {
    super();
    this.workShiftAdjustments = workShiftAdjustments;
  }
  Execute() {}
}
