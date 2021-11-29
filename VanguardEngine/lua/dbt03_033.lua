-- キーンリィ・ルデリィ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dbt01_003")})
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.IsSameZone()
end

function Activation()
	if obj.IsSameZone() then
		obj.Select({q.Location, l.Soul, q.UnitType, u.Normal, q.Min, 0})
		local count = obj.GetNumberOf({q.Location, l.Selected})
		obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.This}, cs.BonusPower, count * 5000, p.UntilEndOfTurn)
		obj.ReturnToDeck({q.Location, l.Selected})
	end
end
