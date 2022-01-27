-- グラビディア・シャーゴ

function RegisterAbilities()
	-- ACT
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnACT)
	ability1.SetLocation(l.RC)
	ability1.SetProperty(p.OncePerTurn)
	ability1.SetCost("Cost")
	ability1.SetActivation("ACT")
end

function Cost(check)
	if check then return obj.CanSB(2) end
	obj.SoulBlast(2)
end

function ACT()
	obj.Select({q.Location, l.Drop, q.Other, o.Meteorite, q.Count, 5})
	local returned = obj.ReturnToDeck({q.Location, l.Selected})
	obj.Store(returned)
	local count = obj.GetNumberOf({q.Location, l.Stored})
	if count >= 1 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
	if count >= 3 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
	if count >= 5 then
		obj.AddCardValue({q.Other, o.This}, cs.BonusPower, 5000, p.UntilEndOfTurn)
	end
end