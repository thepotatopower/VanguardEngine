-- 通貫の騎士 キャドワラ

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function Condition()
	return obj.GetNumberOf({q.Location, l.PlayerUnits}) >= 4
end

function Cost(check)
	if check then return obj.CanSB(1) and obj.CanRetire({q.Other, o.ThisFieldID}) end
	obj.SoulBlast(1)
	obj.Retire({q.Other, o.ThisFieldID})
end

function Activation()
	obj.LookAtTopOfDeck(3)
	obj.DisplayCards({q.Location, l.Looking})
	obj.Store(obj.Search({q.Location, l.Looking, q.Grade, 2, q.Other, o.GradeOrGreater, q.Count, 1, q.Min, 0}))
	if not obj.Exists({q.Location, l.Stored}) then
		obj.Draw(1)
	end
end