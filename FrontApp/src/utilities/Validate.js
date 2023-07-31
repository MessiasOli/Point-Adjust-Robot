const Validate = {
  adjustDate(date){
    let dateSplit = date.trim().split("/")

    if(dateSplit.length != 3)
    throw "Data inválida: " + date

    let day = parseInt(dateSplit[0].trim())
    let month = parseInt(dateSplit[1].trim())
    let year = parseInt(dateSplit[2].trim())

    if(month > 12){

      let aux = day
      day = month
      month = aux;
    }

    let stDay = day < 10 ? "0" + day : day + "";
    let stMonth = month < 10 ? "0" + month : month + "";
    let stYear = year < 100 ? "20" + year : year < 1000 ? "2" + year : year + "";

    return `${stDay}/${stMonth}/${stYear}`
  },

  adjustHour(hourClock){
    let hourClockSplit = hourClock.trim().split(":")

    if(hourClock.toLowerCase() == "cancelar")
      return hourClock;

    if(hourClockSplit.length != 2)
      throw "Hora inválida: " + hourClock

    let hour = parseInt(hourClockSplit[0].trim())
    let minutes = parseInt(hourClockSplit[1].trim())

    let stHour = hour < 10 ? "0" + hour : hour + "";
    let stMinutes = minutes < 10 ? "0" + minutes : minutes + "";

    let time = `${stHour}:${stMinutes}`

    return time;
  },

  removeDoubleSpaceAndReturns(data = String){
    return JSON.parse(JSON.stringify(data).replace(/\\[nrt]/, "").replace(/\s\s/, /\s/))
  },
}

export default Validate
