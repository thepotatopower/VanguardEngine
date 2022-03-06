-- 驚異の急成長 ルジェナ

function RegisterAbilities()
	-- on end of battle
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster() and obj.Exists({q.Location, l.PlayerUnits, q.NameContains, obj.LoadName("Earnescorrect"), q.Other, o.Attacking})
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Clarissa")})
end

function Cost(check)
	if check then return obj.CanSB(1) and obj.CanRetire({q.Other, o.ThisFieldID}) end
	obj.SoulBlast(1)
	obj.Retire({q.Other, o.ThisFieldID})
end

function CanFullyResolve()
	return obj.GetNumberOf("Filter", {l.Deck}) > 0
end

function Filter(id)
	return obj.GradeOf(id) <= 3 and obj.NameContains(id, obj.LoadName("Earnescorrect"))
end

function Activation()
	obj.Search("Filter", {l.Deck}, 1, 0)
end