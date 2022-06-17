import axios, { AxiosError, AxiosResponse } from 'axios';
import { baseUrl } from '../AppSettings';
import { getAccessToken } from '../authentication/Auth';
import createQueryString from '../helpers/createQueryString';
import getFieldMask from '../helpers/getFieldMask';

axios.defaults.baseURL = baseUrl;
const getRespBody = (response: AxiosResponse) => response.data;

axios.interceptors.response.use(
  (response) => {
    return response;
  },
  (error: AxiosError) => {
    if (error.response?.data.detail) {
      return Promise.reject({ message: error.response?.data.detail });
    }
    return Promise.reject(error);
  }
);

const axiosConfig = {
  timeout: 5000
};

function getConfig(accessToken: string = '') {
  if (accessToken === '')
    return {
      timeout: 5000
    };
  else {
    return {
      timeout: 5000,
      headers: { authorization: `bearer ${accessToken}` }
    };
  }
}

const requests = {
  put: (url: string, body: any) =>
    axios.put(url, body, axiosConfig).then(getRespBody),
  patch: (url: string, body: any) =>
    axios.patch(url, body, axiosConfig).then(getRespBody),

  post: async (baseUrl: string, data: any) => {
    const accessToken = await getAccessToken();
    return axios
      .post(baseUrl, { ...data }, getConfig(accessToken))
      .then(getRespBody);
  },

  create: async (baseUrl: string, data: any) => {
    const accessToken = await getAccessToken();
    return axios
      .post(baseUrl, { ...data }, getConfig(accessToken))
      .then(getRespBody);
  },

  get: async (baseUrl: string) => {
    const accessToken = await getAccessToken();
    return axios.get(baseUrl, getConfig(accessToken)).then(getRespBody);
  },

  delete: async (url: string) => {
    const accessToken = await getAccessToken();
    return axios.delete(url, getConfig(accessToken)).then(getRespBody);
  },

  list: async (baseUrl: string, data: any) => {
    const accessToken = await getAccessToken();
    return axios
      .get(baseUrl + createQueryString(data), getConfig(accessToken))
      .then(getRespBody);
  },
  update: async (baseUrl: string, data: any) => {
    const accessToken = await getAccessToken();
    return axios
      .patch(
        baseUrl,
        { FieldMask: getFieldMask(data), ...data },
        getConfig(accessToken)
      )
      .then(getRespBody);
  },
  custom: async (baseUrl: string, data: any) => {
    const accessToken = await getAccessToken();
    return axios
      .post(
        baseUrl,
        { FieldMask: getFieldMask(data), ...data },
        getConfig(accessToken)
      )
      .then(getRespBody);
  }
};

const User = {
  update: async (data: any) => await requests.update(`user`, data),
  list: async (data: any) => await requests.list(`user`, data),
  createSafely: async (data: any) =>
    await requests.custom(`user/:createUserSafely`, data)
};

const Project = {
  get: (id: string) => requests.get(`project/${id}`),
  create: async (data: any) => await requests.create(`project`, data),
  update: async (data: any) => await requests.update(`project`, data),
  list: async (data: any) => await requests.list(`project`, data),
  delete: async (id: string) => await requests.delete(`project/${id}`)
};

const Issue = {
  create: async (data: any) => await requests.create(`issue`, data),
  update: async (data: any) => await requests.update(`issue`, data),
  getIssueKanban: async () =>
    await requests.custom(`issue/:getIssueKanban`, {}),
  updateIssueKanban: async (data: any) =>
    await requests.custom(`issue/:updateIssueKanban`, data),
  get: (id: string) => requests.get(`issue/${id}`),
  list: async (data: any) => await requests.list(`issue`, data),
  delete: async (id: string) => await requests.delete(`issue/${id}`)
};

const Permission = {
  create: async (data: any) => await requests.create(`permission`, data),
  update: async (data: any) => await requests.update(`permission`, data),
  get: (userId: string, issueId: string) =>
    requests.get(`permission/${userId}/${issueId}`),
  list: async (data: any) => await requests.list(`permission`, data),
  delete: async (userId: string, issueId: string) =>
    await requests.delete(`permission/${userId}/${issueId}`)
};

const Comment = {
  create: async (data: any) => await requests.create(`comment`, data),
  update: async (data: any) => await requests.update(`comment`, data),
  list: async (data: any) => await requests.list(`comment`, data),
  delete: async (id: string) => await requests.delete(`comment/${id}`)
};

export const adapter = {
  User,
  Project,
  Issue,
  Permission,
  Comment
};
