import Validate from "src/utilities/Validate";
import AdjustWorkshift from "../../model/AdjustWorkshift";
import Command from "./Command";

export default class CommandAdjust extends Command {
  constructor(workShiftAdjustments = Array) {
    super(workShiftAdjustments);
    this.workShiftAdjustments = this.data.map(a => new AdjustWorkshift(a));
    delete this.data
  }
  Execute() {}
}




