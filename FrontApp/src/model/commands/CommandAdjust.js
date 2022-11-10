import Command from "./Command";

export default class CommandAdjust extends Command {
  constructor(workShiftAdjustments = Array) {
    super();
    let adjustdata = JSON.stringify(workShiftAdjustments).replace(/\\[nrt]/, "").replace(/\s\s/, /\s/)
    workShiftAdjustments = JSON.parse(adjustdata)

    this.workShiftAdjustments = workShiftAdjustments.map(a =>
      ({
        ...a,
        replaceTime: a.replaceTime.trim(),
        hour: adjustHour(a.hour),
        date: adjustDate(a.date),
        reference: adjustDate(a.reference)
      })
    );
  }
  Execute() {}
}

function adjustDate(date){
  let dateSplit = date.split("/")

  if(dateSplit.length != 3)
    throw "Data inválida: " + date

  let day = parseInt(dateSplit[0])
  let month = parseInt(dateSplit[1])
  let year = parseInt(dateSplit[2])

  if(month > 12){
    let aux = day
    day = month
    month = aux;
  }

  let stDay = day < 10 ? "0" + day : day + "";
  let stMonth = month < 10 ? "0" + month : month + "";
  let stYear = year < 100 ? "20" + year : year < 1000 ? "2" + year : year + "";

  return `${stDay}/${stMonth}/${stYear}`
}

function adjustHour(hourClock){
  let hourClockSplit = hourClock.split(":")

  if(hourClockSplit.length != 2)
    throw "Hora inválida: " + hourClock

  let hour = parseInt(hourClockSplit[0])
  let minutes = parseInt(hourClockSplit[1])

  let stHour = hour < 10 ? "0" + hour : hour + "";
  let stMinutes = minutes < 10 ? "0" + minutes : minutes + "";

  return `${stHour}:${stMinutes}`
}
