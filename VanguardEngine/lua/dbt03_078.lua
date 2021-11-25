-- デザイアデビル ゴーマン

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetCost("Cost")
	ability1.SetTiming(a.OnRide)
	ability1.SetTriggerCondition("Trigger")
	ability1.SetActivation("OnRide")
	-- put into soul from RC
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnPut)
	ability2.SetSourceIsPlayer(true)
	ability2.SetSourceLocation(l.VC)
	ability2.SetMovedFrom(l.RC)
	ability2.SetMovedTo(l.Soul)
	ability2.SetTrigger("PutIntoSoulTrigger")
	ability2.SetCondition("PutIntoSoulCondition")
	ability2.SetCost("PutIntoSoulCost")
	ability2.SetActivation("PutIntoSoulActivation")
end

function Trigger()
	return obj.Exists({q.SnapshotIndex, 0, q.Other, o.This}) and obj.Exists({q.SnapshotIndex, 1, q.Name, obj.GetNameFromCardID("dbt03_034")})
end

function Cost(check)
	if check then return obj.CanReveal({q.Location, l.RideDeck, q.Name, obj.GetNameFromCardID("dbt03_003"), q.Count, 1}) end
	obj.ChooseReveal({q.Location, l.RideDeck, q.Name, obj.GetNameFromCardID("dbt03_003"), q.Count, 1})
end

function OnRide()
	obj.Draw(1)
end

function PutIntoSoulTrigger()
	return obj.Exists({q.SnapshotIndex, 0, q.Other, o.This, q.Count, 1}) 
end

function PutIntoSoulCondition()
	return obj.Exists({q.Location, l.EnemyVC, q.Grade, 3, q.Other, o.GradeOrHigher, q.Count, 1})
end

function PutIntoSoulCost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function PutIntoSoulActivation()
	obj.AddPlayerValue(ps.GuardRestrict, 2, p.UntilEndOfTurn, false)
end