import CoverWorkshift from "../../model/CoverWorkshift";
import Command from "./Command";

export default class CommandCover extends Command {
  constructor(coverWorkShifts = Array) {
    super(coverWorkShifts);
    this.coverWorkShifts = this.data.map(d => new CoverWorkshift(d));
    delete this.data
  }
  Execute() {}
}
