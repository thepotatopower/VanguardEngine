-- 怨恨の冥竜神 ゴルマギエルド

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnOverTrigger)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.LastRevealedTrigger()
end

function Activation()
	local newAbility = GiveAbility(GetID(), obj.GetPlayerAbilityID())
	newAbility.SetDescription(2)
	newAbility.SetTiming(a.Cont)
	newAbility.SetActivation("OverTrigger")
end

function OverTrigger()
	obj.AddCardValue({q.Location, l.PlayerVC}, cs.BonusPower, 10000, p.Continuous)
	obj.AddCardValue({q.Location, l.PlayerVC}, cs.BonusCritical, 1, p.Continuous)
end