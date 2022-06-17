import moment from 'moment';

function convertUtcToLocal(dateStr: string): Date {
  let utc = moment.utc(dateStr).toDate();
  let local = moment(utc).local().format('YYYY-MM-DD HH:mm:ss');

  return new Date(local);
}

export default convertUtcToLocal;
