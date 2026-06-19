import moment from 'moment';

function convertUtcToLocal(dateStr: string): Date {
  const utc = moment.utc(dateStr).toDate();
  const local = moment(utc).local().format('YYYY-MM-DD HH:mm:ss');

  return new Date(local);
}

export default convertUtcToLocal;
