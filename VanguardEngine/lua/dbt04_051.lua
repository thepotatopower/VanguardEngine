-- インジェクション・エンジェル

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.Damage, q.Other, o.FaceUp})
end

function Cost(check)
	if check then return obj.CanAddToDamageZone({q.Location, l.Hand, q.UnitType, u.Trigger, q.Count, 1}) end
	obj.ChooseAddToDamageZone({q.Location, l.Hand, q.UnitType, u.Trigger, q.Count, 1}, false)
end

function Activation()
	obj.ChooseAddToHand({q.Location, l.Damage, q.Other, o.FaceUp, q.Count, 1})
end