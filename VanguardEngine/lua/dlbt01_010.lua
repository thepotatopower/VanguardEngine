-- Earnescorrectメンバー エブリン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetMovedFrom(l.Hand)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerVC, q.Name, obj.GetNameFromCardID("dlbt01_001")})
end

function Cost(check)
	if check then return obj.CanCB(1) end
	obj.CounterBlast(1)
end

function CanFullyResolve()
	return obj.GetNumberOf("Filter", {l.Deck}) >= 1
end

function Filter(id)
	return not obj.NameIs(id, obj.GetNames({q.Location, l.PlayerRC})) and obj.GradeOf(id) <= 2 and obj.NameContains(id, obj.LoadName("Earnescorrect"))
end

function Activation()
	obj.Select("Filter", {l.Deck}, 1, 0)
	obj.Reveal({q.Location, l.Selected})
	obj.SuperiorCallToSpecificCircle({q.Location, l.Selected}, FL.OpenCircle)
	obj.Shuffle()
end