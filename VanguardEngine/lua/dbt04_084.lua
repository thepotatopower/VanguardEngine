-- 輝く我欲

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBlitzOrder)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanSB(4) end
	obj.SoulBlast(4)
end

function Activation()
	local power = obj.GetNumberOf({q.Location, l.Damage}) * 5000
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, power, p.UntilEndOfBattle)
end