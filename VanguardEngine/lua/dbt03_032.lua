-- ディアブロスストライカー ブライアン

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function Activation()
	obj.AddCardValue({q.Location, l.FrontRow}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	if obj.InFinalRush() then
		obj.Select({q.Location, l.PlayerVC, q.Count, 1})
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusCritical, 1, p.UntilEndOfTurn)
	end
end