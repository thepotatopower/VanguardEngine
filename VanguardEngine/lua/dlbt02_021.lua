-- 食べ過ぎ注意！ アイリーン

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetLocation(l.RC)
	ability1.SetTiming(a.OnAttack)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsBooster() and obj.Exists({q.Location, l.PlayerRC, q.Other, o.Attacking})
end

function Activation()
	obj.LookAtTopOfDeck(1)
	obj.DisplayCards({q.Location, l.Looking})
	if obj.SelectOption(obj.GetDescription(2), obj.GetDescription(3)) == 1 then
		obj.SendToTop({q.Location, l.Looking})
	else
		obj.SendToBottom({q.Location, l.Looking})
	end
end