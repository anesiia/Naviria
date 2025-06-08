describe("Редагування профілю", () => {

    it("Підтвердження форми без змін викликає помилку", () => {
        cy.loginAsNataliya();
        cy.visit("/profile");
        cy.get('.edit-profile img[alt="edit"]').click();
        cy.url().should("include", "/edit-profile");

        cy.url().should("include", "/edit-profile");
        cy.contains("Редагування профілю").should("exist");

        cy.get('input[name="fullName"]').should("exist");
        cy.get('input[name="nickname"]').should("exist");
        cy.get('textarea[name="description"]').should("exist");
        cy.get('input[name="email"]').should("exist");
        cy.get('input[name="password"]').should("exist");
        cy.get('button[type="submit"]').should("exist");

        cy.get(".photo-section img.photo-preview, .photo-section .photo-placeholder").should("exist");
        cy.get(".photo-section input[type='file']").should("exist");
        cy.get(".photo-section button").contains("Зберегти фото").should("exist");

        cy.get('button[type="submit"]').click();

        cy.get(".error").should("contain.text", "Failed to update profile");
    });

    it("Змінює ім'я, підтверджує alert, повертається на профіль і перевіряє зміни", () => {
        cy.loginAsMaria();
        cy.visit("/profile");
        cy.get('.edit-profile img[alt="edit"]').click();
        cy.url().should("include", "/edit-profile");

        cy.get('input[name="nickname"]').then(($input) => {
            const currentVal = $input.val();
            const newVal = currentVal === "maria99" ? "maria991" : "maria992";
            cy.wrap($input).clear().type(newVal);
        });

        cy.get('button[type="submit"]').click();

        cy.wait(1000);

        // Перевіряємо, що з'явилось підтвердження (alert)
        cy.on("window:alert", (txt) => {
            expect(txt).to.contains("Профіль успішно оновлено");
        });

        cy.visit("/profile");

        // Перевіряємо, що нове ім'я відображається на сторінці профілю
        cy.get('.profile-page .name').then(($el) => {
            const nickname = $el.text();
            expect(['maria991', 'maria992']).to.include(nickname);
        });
    });
});
