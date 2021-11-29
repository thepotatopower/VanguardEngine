-- 厳罰の騎士 ゲイド

function RegisterAbilities()
	-- on retire
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnRetire)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsPayingCost() and obj.IsApplicable()
end

function Activation()
	obj.LookAtTopOfDeck(3)
	local added = obj.ChooseAddToHand({q.Location, l.Looking, q.NameContains, obj.LoadName("Blaster"), q.Count, 1, q.Min, 0})
	obj.Store(added)
	obj.Reveal({q.Location, l.Stored})
	obj.RearrangeOnBottom({q.Location, l.Looking})
end