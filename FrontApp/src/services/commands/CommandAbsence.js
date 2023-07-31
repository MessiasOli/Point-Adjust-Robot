import Absence from "src/model/Absence";
import Command from "./Command";

export default class CommandAbsence extends Command {
  constructor(absence = Array) {
    super(absence);

    this.workshiftAbsences = this.data.map(a => new Absence(a));
    delete this.data
  }
  Execute() { }
}
