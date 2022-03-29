-- クロースロック・ドラゴン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.EnemyRC, q.Other, o.CanChoose})
end

function Activation()
	obj.ChooseMoveEnemyRearguard({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1}, {FL.OpenCircle, FL.FrontRow, FL.EnemyCircle})
end