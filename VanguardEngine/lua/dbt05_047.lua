-- 厳しい修練 テグリア

function RegisterAbilities()
	-- on ride
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRide)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
	-- glitter
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetProperty(p.IsGlitter)
end

function Trigger()
	return obj.WasRodeUponByNameContains("Thegrea") and obj.WasRodeUponByGradeIs(3) 
end

function Activation()
	obj.LookAtTopOfDeck(5)
	obj.Search({q.Location, l.Looking, q.NameContains, "Thegrea", q.NameContains, "Maple", q.Count, 1, q.Min, 0})
	obj.Shuffle()
end