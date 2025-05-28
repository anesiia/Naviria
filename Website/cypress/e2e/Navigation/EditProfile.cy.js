describe("Редагування профілю", () => {

    it("Підтвердження форми без змін викликає помилку", () => {
        cy.loginAsNataliya();
        cy.visit("/profile");
        cy.get('.edit-profile img[alt="edit"]').click();
        cy.url().should("include", "/edit-profile");

        // Переконуємося, що перейшли на сторінку редагування (адреса або заголовок)
        cy.url().should("include", "/edit-profile");
        cy.contains("Редагування профілю").should("exist");

        // Перевіряємо наявність елементів форми
        cy.get('input[name="fullName"]').should("exist");
        cy.get('input[name="nickname"]').should("exist");
        cy.get('textarea[name="description"]').should("exist");
        cy.get('input[name="email"]').should("exist");
        cy.get('input[name="password"]').should("exist");
        cy.get('button[type="submit"]').should("exist");

        // Перевіряємо блок фото
        cy.get(".photo-section img.photo-preview, .photo-section .photo-placeholder").should("exist");
        cy.get(".photo-section input[type='file']").should("exist");
        cy.get(".photo-section button").contains("Зберегти фото").should("exist");

        // Сабмітимо форму без змін
        cy.get('button[type="submit"]').click();

        // Очікуємо, що з'явиться повідомлення про помилку (в компоненті error)
        cy.get(".error").should("contain.text", "Failed to update profile");
    });

    it("Змінює ім'я, підтверджує alert, повертається на профіль і перевіряє зміни", () => {
        cy.loginAsMaria();
        cy.visit("/profile");
        cy.get('.edit-profile img[alt="edit"]').click();
        cy.url().should("include", "/edit-profile");

        // Змінюємо ім'я
        cy.get('input[name="nickname"]').then(($input) => {
            const currentVal = $input.val();
            const newVal = currentVal === "maria99" ? "maria991" : "maria992";
            cy.wrap($input).clear().type(newVal);
        });

        // Сабмітимо форму
        cy.get('button[type="submit"]').click();

        cy.wait(1000);

        // Перевіряємо, що з'явилось підтвердження (alert)
        cy.on("window:alert", (txt) => {
            expect(txt).to.contains("Профіль успішно оновлено");
        });

        // Після підтвердження alert переходимо на сторінку профілю
        cy.visit("/profile");

        // Перевіряємо, що нове ім'я відображається на сторінці профілю
        cy.get('.profile-page .name').then(($el) => {
            const nickname = $el.text();
            expect(['maria991', 'maria992']).to.include(nickname);
        });
    });
});
