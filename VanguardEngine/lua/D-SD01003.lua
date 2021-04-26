-- Blaze Maiden, Rino

function ConditionType()
	return e.OnRide, e.OnAttack
end

function CheckConditionOnRide()
	if obj.isTopSoul() and obj.HasCardInDeck("Trickstar") then
		obj.isSuperiorCall = true
		return true
	else
		return false
	end
end

function CheckConditionOnAttack()
	if obj.isAttackingUnit() then
		obj.needsPrompt = false
		return true
	else
		return false
	end
end

function OnRideActivate(n)
	obj.SuperiorCall(n, "Trickstar")
end

function OnAttackActivate(n)
	obj.addBattleOnlyPower(obj.AttackingUnitID(), 2000)
end

