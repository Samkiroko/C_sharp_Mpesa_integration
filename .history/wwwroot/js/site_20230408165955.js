$(() => {
  $('#register').on('click', async (e) => {
    const _response = await fetch('/register-urls');
    const response = await _response.json();
    console.log(response);
  });
});
