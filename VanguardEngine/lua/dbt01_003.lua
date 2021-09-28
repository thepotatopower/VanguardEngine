-- Master of Gravity, Baromagnes

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Location, l.EnemyRC
	elseif n == 3 then
		return q.Location, l.Soul, q.Count, 2, q.Min, 0
	elseif n == 4 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.CB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() and obj.IsAttackingUnit() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.SoulCount() >= 5 then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.SoulCount() >= 5 then
			obj.Draw(1)
		end
	    if obj.SoulCount() >= 10 then
	    	obj.AddBattleOnlyPower(1, 10000)
	    	obj.AddCritical(1, 1)
	    end
	    if obj.SoulCount() >= 15 then
	    	obj.AddToSoul(2)
	    	obj.Select(3)
	    	obj.SuperiorCall(4)
	    	obj.AddTempPower(4, 10000)
	    	obj.EndSelect()
	    end
	end
	return 0
end