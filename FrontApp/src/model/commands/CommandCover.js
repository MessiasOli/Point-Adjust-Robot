import Command from "./Command";

export default class CommandCover extends Command {
  constructor(coverWorkShifts = Array) {
    super();
    this.coverWorkShifts = coverWorkShifts;
  }
  Execute() {}
}
