import Command from "./Command";

export default class CommandCover extends Command {
  constructor(coverWorkShifts = Array) {
    super();
    let adjustdata = JSON.stringify(coverWorkShifts).replace(/\\[nrt]/, "").replace("  ", " ")
    coverWorkShifts = JSON.parse(adjustdata)
    this.coverWorkShifts = coverWorkShifts;
  }
  Execute() {}
}
