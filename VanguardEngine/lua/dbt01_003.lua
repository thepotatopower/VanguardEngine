-- Master of Gravity, Baromagnes

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 7
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.Damage, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerVC, q.Other, o.This
	elseif n == 3 then
		return q.Location, l.PlayerRC
	elseif n == 4 then
		return q.Location, l.EnemyRC
	elseif n == 5 then
		return q.Location, l.Soul, q.Count, 2, q.Min, 0
	elseif n == 6 then
		return q.Location, l.Soul, q.Count, 1
	elseif n == 7 then
		return q.Location, l.Selected
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, t.Auto, p.HasPrompt, true, p.IsMandatory, false
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsVanguard() and obj.IsAttackingUnit() and obj.CanCB(1) and obj.SoulCount() >= 5 then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Cost(n)
	if n == 1 then
		obj.CounterBlast(1)
	end
end

function Activate(n)
	if n == 1 then
		if obj.SoulCount() >= 5 then
			obj.Draw(1)
		end
	    if obj.SoulCount() >= 10 then
	    	obj.AddBattleOnlyPower(2, 10000)
	    	obj.AddCritical(2, 1)
	    end
	    if obj.SoulCount() >= 15 then
	    	obj.AddToSoul(3)
	    	obj.AddToEnemySoul(4)
	    	if obj.Exists(6) then
	    		obj.Select(5)
	    		obj.SuperiorCall(7)
	    		obj.AddTempPower(7, 10000)
	    		obj.EndSelect()
	    	end
	    end
	end
	return 0
end