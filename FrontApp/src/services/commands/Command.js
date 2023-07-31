import Validate from "src/utilities/Validate";
import { Auth } from "../../config/global";

export default class Command {
  constructor(dataArray = Array) {
    this.data = Validate.removeDoubleSpaceAndReturns(dataArray)
    this.settings = Auth.getEncrypted();
  }
  Execute() {}
}
