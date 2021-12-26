-- サンバースト・エヴォリューション

function RegisterAbilities()
	-- order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOrder)
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function Activation()
	obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	obj.ChooseAddToHand({q.Location, l.Drop, q.Name, obj.GetNameFromCardID("dsd01_006"), q.Count, 1})
end