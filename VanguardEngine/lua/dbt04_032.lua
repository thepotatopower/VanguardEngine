-- 焔の根僧 レツジ

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.CanAddToHand({q.Location, l.Drop, q.UnitType, u.overDress, q.Count, 1})
end

function Activation()
	obj.ChooseAddToHand({q.Location, l.Drop, q.UnitType, u.overDress, q.Count, 1, q.Min, 0})
	if obj.Exists({q.Location, l.PlayerVC, q.Grade, 4}) then
		obj.CounterCharge(1)
	end
end