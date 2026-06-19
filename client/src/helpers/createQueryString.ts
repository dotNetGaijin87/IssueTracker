function createQueryString(resource: object | undefined): string {
  if (resource === undefined) return '';
  let queryString: string = '?';

  for (const [key, value] of Object.entries(resource)) {
    if (queryString.length > 1 && value) {
      queryString += '&';
    }
    if (value) {
      queryString += `${key}=${value}`;
    }
  }

  return queryString;
}
export default createQueryString;
