import { Auth } from "../../config/global";

export default class Command {
  constructor() {
    this.settings = Auth.getEncrypted();
  }
  Execute() {}
}
